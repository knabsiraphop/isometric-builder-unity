using System.Collections.Generic;
using System.Linq;

namespace IsoBuilder.Core
{
    public class PlaceableCatalog
    {
        private Dictionary<int, PlaceableData> placeableDatas;
        private Dictionary<int, StationType> stationTypes;

        public void Initialize(PlaceableConfig placeableConfig, StationConfig stationConfig)
        {
            placeableDatas = new Dictionary<int, PlaceableData>();
            stationTypes = new Dictionary<int, StationType>();

            IOrderedEnumerable<PlaceableData> sortDatas = placeableConfig.PlaceableDatas.OrderBy(data => data.Id);

            foreach (PlaceableData data in sortDatas)
            {
                placeableDatas[data.Id] = data;
            }

            foreach (StationData station in stationConfig.StationDatas)
            {
                stationTypes[station.Id] = station.Type;
            }
        }

        public bool TryGetPlaceableData(int id, out PlaceableData placeableData)
        {
            return placeableDatas.TryGetValue(id, out placeableData);
        }

        public bool TryGetStationType(int id, out StationType stationType)
        {
            return stationTypes.TryGetValue(id, out stationType);
        }

        public IEnumerable<PlaceableData> GetPlaceableDatas()
        {
            return placeableDatas.Values;
        }
    }
}
