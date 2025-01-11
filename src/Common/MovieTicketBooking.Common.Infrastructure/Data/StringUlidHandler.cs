using System.Data;
using static Dapper.SqlMapper;

namespace MovieTicketBooking.Common.Infrastructure.Data;
public class StringUlidHandler : TypeHandler<Ulid>
{
    public override Ulid Parse(object value)
    {
        return Ulid.Parse((string)value);
    }

    public override void SetValue(IDbDataParameter parameter, Ulid value)
    {
        parameter.DbType = DbType.StringFixedLength;
        parameter.Size = 26;
        parameter.Value = value.ToString();
    }
}

