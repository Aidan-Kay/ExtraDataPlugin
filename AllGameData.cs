using ACSharedMemory.ACC.Reader;
using IRacingReader;
using iRacingSDK;
using GameReaderCommon;

namespace AidanKay.ExtraDataPlugin
{
    internal class AllGameData
    {
        public GameData GameData { get; set; }
        public ACCRawData AccRawData { get; set; }
        public ACCRawData AccOldRawData { get; set; }
        public IRacingReader.DataSampleEx IRacingRawData { get; set; }
        public IRacingReader.DataSampleEx IRacingOldRawData { get; set; }

        public void AssignSpecificGameData()
        {
            if (GameData == null)
                return;

            if (GameData.GameName == "AssettoCorsaCompetizione")
            {
                GameData<ACCRawData> accData = GameData as GameData<ACCRawData>;
                AccRawData = accData.GameNewData.Raw;
                AccOldRawData = accData.GameOldData.Raw;
            }

            if (GameData.GameName == "IRacing")
                IRacingRawData = GameData.NewData.GetRawDataObject() as DataSampleEx;
                IRacingOldRawData = GameData.OldData.GetRawDataObject() as DataSampleEx;
        }
    }
}
