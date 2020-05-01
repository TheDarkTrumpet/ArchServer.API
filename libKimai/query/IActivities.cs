using System;
using System.Collections.Generic;
using libKimai.models;
using MySql.Data.MySqlClient;

namespace libKimai.query
{
    public interface IActivities
    {
        DateTime? FromDate { get; set; }
        List<Activity> GetActivities(bool orderByDesc = false);
        string BuildStatement(bool orderByDesc = false);
        Activity CreateActivity(MySqlDataReader record);
    }
}