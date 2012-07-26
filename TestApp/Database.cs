using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

// Entity definitions

public class Players
{
    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("nameFirst")]
    public string FirstName { get; set; }

    [Column("nameLast")]
    public string LastName { get; set; }

    [Column("bats")]
    public string Bats { get; set; }

    [Column("throws")]
    public string Throws { get; set; }

    [Column("nameNick")]
    public string NickName { get; set; }

    // TODO: how to convert this to DateTime?
    [Column("debut")]
    public long Debut { get; set; }

    // TODO: how to convert this to DateTime?
    [Column("finalGame")]
    public long FinalGame { get; set; }

    [Column("birthDay")]
    public int BirthDay { get; set; }
    
    [Column("birthMonth")]
    public int BirthMonth { get; set; }

    [Column("birthYear")]
    public int BirthYear { get; set; }

    private PlayerBattingStats _battingStats;
    private PlayerPitchingStats _pitchingStats;

    public async Task<PlayerBattingStats> GetBattingStatsAsync()
    {
        if (_battingStats == null)
        {
            _battingStats = (await BaseballDatabase.GetDatabaseAsync()).GetBattingStats(PlayerID);
        }
        return _battingStats;
    }

    public async Task<PlayerPitchingStats> GetPitchingStatsAsync()
    {
        if (_pitchingStats == null)
        {
            _pitchingStats = (await BaseballDatabase.GetDatabaseAsync()).GetPitchingStats(PlayerID);
        }
        return _pitchingStats;
    }
}

public class Master
{
    [Column("lahmanID")]
    public int LahmanID { get; set; }

    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("birthState")]
    public string BirthState { get; set; }

    [Column("nameFirst")]
    public string FirstName { get; set; }

    [Column("nameLast")]
    public string LastName { get; set; }
}

public class PlayerPitchingStats
{
    public List<PitchingStats> PitchingStats { get; set; }

    public PlayerPitchingStats(List<PitchingStats> pitchingStats)
    {
        PitchingStats = pitchingStats;
    }

    public int CareerWins
    {
        get
        {
            return PitchingStats.Sum(item => item.Wins);
        }
    }

    public int CareerLosses
    {
        get
        {
            return PitchingStats.Sum(item => item.Losses);
        }
    }


    public int CareerSaves
    {
        get
        {
            return PitchingStats.Sum(item => item.Saves);
        }
    }

    public int CareerInningsPitchedOuts
    {
        get
        {
            return PitchingStats.Sum(item => item.InningsPitchedOuts);
        }
    }

}

/// <summary>
/// Class that encapsulates both individual batting stat entries as well
/// as cumulative totals of those stats (eg career home runs)
/// </summary>
public class PlayerBattingStats
{
    private List<BattingStats> BattingStats { get; set; }

    public PlayerBattingStats(List<BattingStats> battingStats)
    {
        BattingStats = battingStats;
    }

    public int CareerHomeRuns
    {
        get
        {
            return BattingStats.Sum(item => item.HomeRuns);
        }
    }

    public int CareerAtBats
    {
        get
        {
            return BattingStats.Sum(item => item.AtBats);
        }
    }

    public int CareerHits
    {
        get
        {
            return BattingStats.Sum(item => item.Hits);
        }
    }

    public int CareerDoubles
    {
        get
        {
            return BattingStats.Sum(item => item.Doubles);
        }
    }

    public int CareerTriples
    {
        get
        {
            return BattingStats.Sum(item => item.Triples);
        }
    }

    public double CareerWalks
    {
        get
        {
            return BattingStats.Sum(item => item.Walks);
        }
    }

    public double CareerBattingAverage
    {
        get
        {
            return (double)CareerHits / (double)CareerAtBats;
        }
    }
}

public class BattingStats
{
    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("yearID")]
    public int Year { get; set; }

    [Column("AB")]
    public int AtBats { get; set; }

    [Column("H")]
    public int Hits { get; set; }

    [Column("2B")]
    public int Doubles { get; set; }

    [Column("3B")]
    public int Triples { get; set; }

    [Column("HR")]
    public int HomeRuns { get; set; }

    [Column("BB")]
    public int Walks { get; set; }

    [Column("SO")]
    public int StrikeOuts { get; set; }

    public double BattingAverage
    {
        get
        {
            return (double)Hits / (double)AtBats;
        }
    }
}

public class PitchingStats
{
    [Column("playerID")]
    public string PlayerID { get; set; }

    [Column("yearID")]
    public int Year { get; set; }

    [Column("stint")]
    public int Stint { get; set; }

    [Column("W")]
    public int Wins { get; set; }

    [Column("L")]
    public int Losses { get; set; }

    [Column("G")]
    public int Games { get; set; }

    [Column("GS")]
    public int GamesStarted { get; set; }

    [Column("CG")]
    public int CompleteGames { get; set; }

    [Column("SHO")]
    public int Shutouts { get; set; }

    [Column("SV")]
    public int Saves { get; set; }

    [Column("GF")]
    public int GamesFinished { get; set; }

    [Column("IPOuts")]
    public int InningsPitchedOuts { get; set; }

    [Column("H")]
    public int Hits { get; set; }

    [Column("R")]
    public int Runs { get; set; }

    [Column("ER")]
    public int EarnedRuns { get; set; }

    [Column("GIDP")]
    public int GroundIntoDoublePlays { get; set; }

    [Column("SH")]
    public int SacrificeHits { get; set; }

    [Column("SF")]
    public int SacrificeFlies { get; set; }

    [Column("HR")]
    public int HomeRuns { get; set; }

    [Column("BB")]
    public int Walks { get; set; }

    [Column("IBB")]
    public int IntentionalWalks { get; set; }

    [Column("WP")]
    public int WildPitches { get; set; }

    [Column("BK")]
    public int Balks { get; set; }

    [Column("BFP")]
    public int BattersFaced { get; set; }

    [Column("SO")]
    public int Strikeouts { get; set; }

    [Column("ERA")]
    public double EarnedRunAverage { get; set; }
}

public class BaseballDatabase
{
    private static BaseballDatabase _current = null;

    const string BASEBALL_DATABASE = "baseball-archive-2011.sqlite";

    // Note: I can't implement the try pattern because async methods cannot have out or ref params
    // however, I need a name that suggests that there is a distinguished return value from the function
    // if something expected (database not there) happens. 
    private async Task<StorageFile> GetDatabase()
    {
        try
        {
            var pathToApplicationDatabase = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, BASEBALL_DATABASE);
            return await StorageFile.GetFileFromPathAsync(pathToApplicationDatabase);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private async Task<string> GetDatabasePathAsync()
    {
        StorageFile database = await GetDatabase();
        if (database == null)
        {
            // copy the database to the application folder and return a reference to it
            var pathToPackageDatabase = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, BASEBALL_DATABASE);
            var packageDatabase = await StorageFile.GetFileFromPathAsync(pathToPackageDatabase);
            database = await packageDatabase.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
        }
        return database.Path;
    }

    private SQLiteConnection _connection;

    public BaseballDatabase() { }

    // TODO: rewrite this ... there are races here
    public static async Task<BaseballDatabase> GetDatabaseAsync()
    {
        if (_current == null)
        {
            var database = new BaseballDatabase();
            var path = await database.GetDatabasePathAsync();
            database._connection = new SQLiteConnection(path);
            _current = database;
        }
        return _current;
    }

    // TODO: should these be async methods?
    public PlayerBattingStats GetBattingStats(string playerId)
    {
        var battingStats = _connection.Query<BattingStats>("select * from batting where playerId=?", playerId);
        return new PlayerBattingStats(battingStats);
    }

    public PlayerPitchingStats GetPitchingStats(string playerId)
    {
        var pitchingStats = _connection.Query<PitchingStats>("select * from pitching where playerId=?", playerId);
        return new PlayerPitchingStats(pitchingStats);
    }

    public List<Players> GetPlayersBornInState(string stateId)
    {
        return _connection.Query<Players>("select * from master where birthState=?", stateId);
    }

    public List<Players> GetPlayersByName(string firstName, string lastName)
    {
        return _connection.Query<Players>("select * from master where nameFirst=? and nameLast=?", firstName, lastName);
    }

    public Players GetPlayerById(string playerId)
    {
        var players = _connection.Query<Players>("select * from master where playerId=?", playerId);
        if (players.Count != 1)
        {
            return null;
        }
        else
        {
            return players[0];
        }
    }
}