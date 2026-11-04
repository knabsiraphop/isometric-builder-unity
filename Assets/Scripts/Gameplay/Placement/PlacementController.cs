using Cysharp.Threading.Tasks;
using IsoBuilder.Core;
using UnityEngine;

namespace IsoBuilder.Gameplay
{
    public class PlacementController : MonoBehaviour
    {
        private struct CurrentSelectedObjectData
        {
            public bool hasSnapShot;
            public Vector2Int cellOrigin;
            public bool flipped;
            public Vector3 worldPosition;
        }

        [SerializeField] private CellHighlighter cellHighlighter;

        private IUIManager uiManager;
        private PlaceableCatalog placeableCatalog;
        private ISpriteLoader spriteLoader;
        private MapController mapController;
        private ObjectManager objectManager;
        private CameraController cameraController;
        private PointerInput pointerInput;
        private PlaceableObjectDragger placeableObjectDragger;
        private PlaceableObjectPressDetector placeableObjectPressDetector;

        private PlacedObject currentPlacedObject;
        private CurrentSelectedObjectData currentSelectedObjectData;

        public void Initialize(IUIManager uiManager, PlaceableCatalog placeableCatalog, ISpriteLoader spriteLoader,
            MapController mapController, ObjectManager objectManager, CameraController cameraController)
        {
            this.uiManager = uiManager;
            this.placeableCatalog = placeableCatalog;
            this.spriteLoader = spriteLoader;
            this.mapController = mapController;
            this.objectManager = objectManager;
            this.cameraController = cameraController;

            pointerInput = new PointerInput();
            placeableObjectDragger = new(cameraController, pointerInput, mapController);
            placeableObjectPressDetector = new(pointerInput, SelectObject, TapObject);
        }

        public async UniTask PlaceObjectFromData(PlacedObjectData placedObjectData)
        {
            objectManager.SetCurrentInstanceId(placedObjectData.InstanceId);
            if (!placeableCatalog.TryGetPlaceableData(placedObjectData.Id, out PlaceableData placeableData))
            {
                Debug.LogError($"placeable data not found : {placedObjectData.Id}");
                return;
            }
            Sprite placeableSprite = await spriteLoader.GetSprite(placeableData.GetResourcePath(), placeableData.GetSpriteKey());
            if (!placeableSprite)
            {
                return;
            }
            if (!objectManager.CanPlaceObject(placeableData, placedObjectData.CellOrigin, placedObjectData.Flipped, placedObjectData.InstanceId))
            {
                Debug.LogError("Can't Place Saved Placeable");
                return;
            }
            if (!objectManager.TryCreatePlacedObject(placeableData, placeableSprite, out PlacedObject placedObject, placedObjectData.InstanceId))
            {
                return;
            }
            SetPlaceablePlacement(placedObjectData.CellOrigin, placedObject, placedObjectData.Flipped);

            objectManager.OnConfirmPlacedObject(placedObject);
            placedObject.InitializePointer(placeableObjectPressDetector.OnObjectPressed);
            return;
        }

        public async UniTask PlaceObject(PlaceableData placeableData)
        {
            Sprite placeableSprite = await spriteLoader.GetSprite(placeableData.GetResourcePath(), placeableData.GetSpriteKey() );
            if (!placeableSprite)
            {
                return;
            }
            if (objectManager.TryCreatePlacedObject(placeableData, placeableSprite, out PlacedObject placedObject))
            {
                Vector2 screenCenter = cameraController.GetScreenCenter();
                Vector2 worldPosition = cameraController.ScreenToWorldPosition(screenCenter);
                Vector2Int cellOrigin = mapController.WorldToCell(worldPosition);
                SetPlaceablePlacement(cellOrigin, placedObject, false);
                SelectObject(placedObject);
            }
        }

        private void SelectObject(PlacedObject placedObject)
        {
            if (placedObject == currentPlacedObject)
                return;

            if (currentPlacedObject != null)
            {
                ClearCurrentPlacement();
            }
            SetHighlight(placedObject);
            currentPlacedObject = placedObject;
            uiManager.HidePlaceableCatalog();
            uiManager.ShowPlacementPanel(ConfirmPlacement, CancelPlacement, FlipPlacement);
            if (objectManager.IsPlaceableObjectConfirm(placedObject))
            {
                currentSelectedObjectData.hasSnapShot = true;
                currentSelectedObjectData.cellOrigin = placedObject.CellOrigin;
                currentSelectedObjectData.flipped = placedObject.Flipped;
                currentSelectedObjectData.worldPosition = placedObject.WorldCellPosition;
            }
        }

        private void TapObject(PlacedObject placedObject)
        {
            if (currentPlacedObject != null)
                return;
            if (placedObject.PlaceableData.Type != PlaceableType.Station)
                return;
            if (placeableCatalog.TryGetStationType(placedObject.PlaceableData.Id, out StationType stationType))
            {
                uiManager.ShowStationPanel(stationType);
            }
        }

        private void ConfirmPlacement()
        {
            if (!objectManager.CanPlaceObject(currentPlacedObject, currentPlacedObject.CellOrigin, currentPlacedObject.Flipped))
            {
                Debug.LogError("Can't Place Saved Placeable");
                return;
            }
            if (objectManager.IsPlaceableObjectConfirm(currentPlacedObject))
                objectManager.OnConfirmMovePlacedObject(currentPlacedObject, currentSelectedObjectData.cellOrigin, currentSelectedObjectData.flipped);
            else
                objectManager.OnConfirmPlacedObject(currentPlacedObject);
            currentPlacedObject.InitializePointer(placeableObjectPressDetector.OnObjectPressed);
            currentSelectedObjectData.hasSnapShot = false;
            ClearCurrentPlacement();
        }

        private void CancelPlacement()
        {
            ClearCurrentPlacement();
        }

        private void FlipPlacement()
        {
            currentPlacedObject.SetPlacement(currentPlacedObject.CellOrigin, !currentPlacedObject.Flipped, currentPlacedObject.WorldCellPosition);
            SetHighlight(currentPlacedObject);
        }

        private void ClearCurrentPlacement()
        {
            if (currentPlacedObject == null)
                return;
            if (!objectManager.IsPlaceableObjectConfirm(currentPlacedObject))
            {
                objectManager.ReturnPlacedObject(currentPlacedObject);
            }
            else
            {
                if (currentSelectedObjectData.hasSnapShot)
                {
                    currentPlacedObject.SetPlacement(currentSelectedObjectData.cellOrigin, currentSelectedObjectData.flipped,
                        currentSelectedObjectData.worldPosition);
                    currentSelectedObjectData.hasSnapShot = false;
                }
            }

            currentPlacedObject = null;
            cellHighlighter.ClearAllHighlights();
            uiManager.ShowPlaceableCatalog();
            uiManager.HidePlacementPanel();
            placeableObjectDragger.Reset();
        }

        private void Update()
        {
            if (pointerInput == null)
                return;
            if (pointerInput.WasPressedThisFrame())
            {
                placeableObjectDragger.OnPressedThisFrame(currentPlacedObject);
                placeableObjectPressDetector.OnPressedThisFrame();
            }

            if (pointerInput.IsPressed())
            {
                if (placeableObjectDragger.TryGetNewCellOrigin(currentPlacedObject, out Vector2Int newCellOrigin))
                {
                    SetPlaceablePlacement(newCellOrigin, currentPlacedObject, currentPlacedObject.Flipped);
                    SetHighlight(currentPlacedObject);
                }
                placeableObjectPressDetector.OnPressed();
            }

            if (pointerInput.WasReleasedThisFrame())
            {
                placeableObjectDragger.OnReleasedThisFrame();
                placeableObjectPressDetector.OnReleasedThisFrame();
            }
        }

        private void SetPlaceablePlacement(Vector2Int cellOrigin, PlacedObject placedObject, bool flipped)
        {
            Vector3 worldCellPosition = mapController.CellToWorld(cellOrigin);
            placedObject.SetPlacement(cellOrigin, flipped, worldCellPosition);
        }

        private void SetHighlight(PlacedObject placedObject)
        {
            cellHighlighter.ClearAllHighlights();
            bool canPlace = objectManager.CanPlaceObject(placedObject, placedObject.CellOrigin, placedObject.Flipped);
            cellHighlighter.HighlightCells(placedObject.PlaceableData.GetCells(placedObject.CellOrigin, placedObject.Flipped), canPlace);
        }
    }
}
