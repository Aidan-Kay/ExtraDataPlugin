using ACSharedMemory.ACC.Reader;
using IRacingReader;
using iRacingSDK;
using GameReaderCommon;

namespace AidanKay.ExtraDataPlugin
{
    internal class AllGameData
    {
        public GameData GameData { get; set; }
        public ACCRawData AccRawGameData { get; set; }
        public IRacingReader.DataSampleEx IRacingRawGameData { get; set; }

        public void AssignSpecificGameData()
        {
            if (GameData == null) return;

            if (GameData.GameName == "AssettoCorsaCompetizione")
            {
                GameData<ACCRawData> accData = GameData as GameData<ACCRawData>;
                AccRawGameData = accData.GameNewData.Raw;
            }

            if (GameData.GameName == "IRacing")
            {
                IRacingRawGameData = GameData.NewData.GetRawDataObject() as DataSampleEx;
            }
        }
    }
}
