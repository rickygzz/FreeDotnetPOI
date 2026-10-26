namespace NPOI.SS.UserModel.Charts;

public interface IPieChartSeries<Tx, Ty> : IChartSeries
{
    /// <summary>
    /// Return data source used for category axis data.
    /// </summary>
    IChartDataSource<Tx> GetCategoryAxisData();

    /// <summary>
    /// Return data source used for value axis.
    /// </summary>
    IChartDataSource<Ty> GetValues();

}
