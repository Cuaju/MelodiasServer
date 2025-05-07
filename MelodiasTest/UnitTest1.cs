using System.Configuration;
using Xunit;

public class ConexionTests
{
    [Fact]
    public void Conexion_DB_Exitosa()
    {
        var cadena = ConfigurationManager.ConnectionStrings["MelodiasContext"];
        Assert.NotNull(cadena);

        using (var ctx = new DataAccess.MelodiasContext())
        {
            ctx.Database.Connection.Open();
            Assert.Equal(System.Data.ConnectionState.Open, ctx.Database.Connection.State);
        }
    }
}
