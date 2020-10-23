using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class ChallengeData
{
    public bool success;
    public string message;
    public Challenge data;

    public ChallengeData(bool success, string message, Challenge data)
    {
        this.success = success;
        this.message = message;
        this.data = data;
    }
}
[Serializable]
class ChallengeListData
{
    public bool success;
    public string message;
    public Challenge[] data;

    public ChallengeListData(bool success, string message, Challenge[] data)
    {
        this.success = success;
        this.message = message;
        this.data = data;
    }
}

