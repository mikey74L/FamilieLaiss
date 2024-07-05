namespace VideoConvertExecuteService.Models;

public class MediaInfoData(int width, int height, string scanType, string scanOrder, int durationSecondsTotal)
{
    #region Properties
    public int Width { get; private set; } = width;

    public int Height { get; private set; } = height;

    public string ScanType { get; private set; } = scanType;

    public string ScanOrder { get; private set; } = scanOrder;

    public int DurationSecondsTotal { get; private set; } = durationSecondsTotal;

    public int IntervalThumbnails
    {
        get
        {
            if (DurationSecondsTotal <= 10 * 60)
            {
                //Interval until 10 minutes
                return 2;
            }
            else
            {
                if (DurationSecondsTotal is > 10 * 60 and <= 30 * 60)
                {
                    //Interval between 10 und 30 minutes
                    return 5;
                }
                else
                {
                    if (DurationSecondsTotal is > 30 * 60 and <= 60 * 60)
                    {
                        //Interval between 30 und 60 minutes
                        return 10;
                    }
                    else
                    {
                        if (DurationSecondsTotal > 60 * 60)
                        {
                            //Interval bigger than one hour
                            return 20;
                        }
                        else
                        {
                            return 20;
                        }
                    }
                }
            }
        }
    }
    #endregion
}