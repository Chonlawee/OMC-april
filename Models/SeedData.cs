using Microsoft.EntityFrameworkCore;
using OMC.Data;


namespace OMC.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OMCContext>();
            {
                if (context == null || context.Product == null)
                {
                    throw new ArgumentNullException("Null OMCContext");
                }


                // Look for any movies.
                if (context.Product.Any())
                {
                    return;   // DB has been seeded
                }

                context.Product.AddRange(
                    new Product
                    {
                        ProductName = "เอสเปรสโซ่",
                        ProductPrice = 45,
                        ProductImage = "iVBORw0KGgoAAAANSUhEUgAAAFkAAABRCAYAAAC5WArCAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAVuSURBVHgB7Z1NbxtFGMf/G6dvCQKTqBGIFrZCvUCl9FYOSHVuCA4tN26Yb1BOcMN8gqafAEtckXiR6NWuxKFwwamIEAnIGzWhkCqJUyWlpaDtPPtibWZn1/Z6PLV3n580XvfZ2Sr79/i/M7PPji3EqQSFidMRpSVKExmxLaAhisulZ2lPAZ+RZoMIXKYDJ+gkx0Zsod3VXuKW6EV8Kp/0U5mJQY3zA6EfXOBWUqVSsK3TAWCyUkkT2qIK4qUh7xAHfI4CIM79ithclMItcf7fKqovWinfeHHMEhIuilXZa0TsGxSHq4rzb6TUpw7C1wkevQeFI0wl/Ed7KA4tOWCl9xoc8Sm8n/BNLwtBr8vBJJGZ3tSgsAYhfhXSOINFHgJq0fAHKEew/D50FxZ5ODoJtlFBxJtZ5OFZhqI1C66Fb1hkDYjWfEOOCcu4HL5nkfWwrIhVEFgGi6wHsoumIu4NXFhkTQjLWFGEvZEki6yPphwQvrxIWxZZHy1FzKYXFlkfjiJm0wuLrBdHEXuNRTYAi2wAFtkALLIBWGS92IrYPousj6Qb0R0WWR8XFTFvgMIi6yMmspjP2KAti6yJ6PxxhCa9sMj6qChibBcaoXlj+cLXnWNmkTUw5WchHcGNJAixyMNjB7kWMt00r2nVUcLEy25xEsFtDAHlKbvxsAMp1a06Bnm+Y1WQngvXUztILZvtIjuUeHhdEXfgpyJ3YZGzEaYbx4bSqoyiaTAq9hPilOx9xY1kB0Vx/RZcl+Mksi0H52ZPYH7mBIrAP0/+w2bn4ZFYkBjuquq7SMQR5WPVDmVLfstewLsXzqIIrG/v40ZjFUPiBFn2qpw49uRhEeI2A4GdpDoscnbCtNlUgQm+8A2A64tJhUZzdSTYgwyLrMD1J3aWoAm2CwOwyAZgkQ3AIhuARTYAi2wAFtkALLIBWGQDsMgGIJFj42+aYy0KJs5VKfLO4WMUhZ2Df1VhBxohkWOPRm12DlEUdh8+UoU3oBES2ZGDu6IlF8Uy6M6IghY0EtqFI+9Y2dxF3qHGJN/fC9AuMlQrR93Zyr/Ia8mt2IFGwi5cbPWs9e0HubeM5tqfsVjaInpZCUVuQuplkMCN3+4hr5AXJ1hFHZrpDkZUq4801u7ltjXfXL2rCreg2Y+J6IivLu8kgb//5S7yxu32tmeHMqqGpoNS5D3ZxTlLesDE2TnA+YXnMT97EnmAehRf/vS7aED/y7scUT7CCJDnLmpQjAD9PyoftvHVz21PaJlRrk1akv5NAj8WrfmdaJA+9V//2sPbr7+ESYZ8+Ic//o7Fg0RBYyITty0/y96OBh88eoIdMQRdfGUekwgJfFN9fXHg20RfiSpZKCXEb1l+tvgRI94SXR6a13jj5TKOlSZnljRFYGrFlInZxAixUvYp11UmKLX22tIFbzvO0HWExKWuqIrAh2sYMaWUfY4oG5ZiUWby6BUx7D51bBpnXpzFOLK1d4jlxqq4lqhdwJTARKnHfuqYJwpN8xvk02fLz+HU8fFIq6PW+92djaRumkfQH/4UhrD6rFcNHkJJWm4Al86dxntvvvrMLMSbBhC2QFMBad1Nky04pF+RCTvwaDutEg1cLtkLWDwz59nJqKE5CLKuH9v3e/XlO8FFrg7DDCIyYYtSEwd92E9lEvz86ReEb89gbubk0P5Nt8W2OgfeLaP1+/uDzBS2ggWlHTwDBhU5pGpl+MUYglr3zPFel4I4Ge87dgL/rWGCIbHbpp8Y7aPQDx/UkLPfThkXsXMprkxFlLphwUnYLzDGD9xn9eR+sOGfOE2dXrbUCyFloROsJUHrFTcxgkl23YxSZBn6CpPQNga/YIZ31LXf5DTBUykK6s2XDmf6AAAAAElFTkSuQmCC",
                    },
                    new Product
                    {
                        ProductName = "อเมริกาโน่",
                        ProductPrice = 45,
                        ProductImage = "iVBORw0KGgoAAAANSUhEUgAAAEgAAABRCAYAAABxNOAUAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAQwSURBVHgB7ZxLTxNRFMf/pfUBBEWJRKOEYYExSqIr48661Y0uXYHfAL+AYvwCmLgwbiTRpUZNlG3ZmCCJkRI0aFEm8jChKRQiKlFT75mHKXfu9LS8LJ3zS26GnjvTpL+ec+fO0LkxBEl6LYrYqg152wBWDEipVoh6qwNu+lLi/h+q463anIFAJFVbUm3YeUXGdsq3u41tUalp9jNogF5AKGavaqsJuOlk6b0F4BYihMqY02pzWYudp22Pnl4q9gzRwzKU2VRdyM6LiB62IWbVQSiJCGIQQQwiiEEEMYggBhHEIIIYRBCDCGIQQQwiiEEEMYgghoQeaD9xAr39/cnD7e0pRIz3IyO43d29JhYQ1NjUBCXHguEuY61z8uzZQExKjEEEMYggBhHEIIIYRBCDCGIQQQwiiEEEMYggBhFUGjtwsZqdm8Pju3chuMTg/vzlAQQTTgZZevRg4x60NOxB1MhklwOxhGnHc1YrLna1IUosrKzixos3gbgM0gwiiEEEMYggBhHEIIIYRBCDCGIQQQwiiEEEMYggBhHEIIIYSFBeD/749RtRI+Qz542CcureSNTIrfwMxAqeoFG9Yya/gqgxs/jdFE6TIFuP0t21qJVZSFLYfonZek96ZgFRIjO/bAqPOmcxVWvP9Z7X9jyiwthszlQxlDhD/mk+8JQzGY1KmaU+fg3ECp4TX9AQDGez1IfggbUGjbch5eVU1b+JojJ2R9+DzC7U+Cn/5fi0KWxDyyCiH1oWUYk9HMmgVhmemjeOtcWrTsSL4jRTqo9pawdRBtXvTqCjpQm1BH2uhyOTKgn+6F22atf8F/q1WCCLiEGVhrVWalQZps+kr1miC8oXiuz5UKn1p8ZrRtLgu2njwOyduQaKY3HD8RPKWge0xZYoFdOzCzh9tMUpuZ0KyRkMH5ivQqugWMj7NHsrUll6B/3yo/dCl7PdaTx5O2Wc8xAqe67AMB+Mh7wXDdjPY+56OmsWXvIzqX5XAscONGInQEPD/VcTePMlZ+z3xp17pr6wDPI5o3ag58aMq1Mljx/Bpa42R1a1klaXEY9efwq9KvDk9IUdzwkiSNJThDw/RqV28VQbznW0oppwT+OZsFmyAyeHKEcQYXmZZIXtUC2iMvNLagKYZS+2lZzrcKc1JSlXEGGp1qcO6C61E4nqbN3n/Eqts3U/tgMqn+HPWYzN5UpmjIftTWWGUAaVCPLpjbkLMbKr5pGsY82N6Dy0Xw3oDer1XrRs8OxHMnLfVjGrbnBNqzaZXQq7GxjAm+eQnDzKZD2CCAtlZFMYNKg37I5XfNwG7pVXlDWbiYXqXt6UFqvrQxVgqTZQLWLgnlB6UIXQuNSj2lP69v6DlD5s8uPs6x2DyiUJ95rufMzdWtgkCu54koa7pVb2wFsJWy1Ix9JaJeS9Rv/Hs7FFQnT+Ai+1NmxsftdIAAAAAElFTkSuQmCC",
                    },
                    new Product
                    {
                        ProductName = "ชา",
                        ProductPrice = 45,
                        ProductImage = "iVBORw0KGgoAAAANSUhEUgAAAEEAAABTCAYAAADA6WrVAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAANISURBVHgB7ZpNaxNRFIbfsQVbBU0rumgRBy2IFFFRCq6aLgR3utSV3bkR9B80/gIV3LjTjYgrl4ILGyyClUKl1FW1EZT6gU38ohWr8ZwzE0knd9o0aWsyeR843OmdmaT3mXvvzJlcD8tJSZyV8JFcJiUeOPdsAS57QF6i2AIxi+BiLxMw0kQNWLeQph/V9ss2/NBMKzIqJoakEwQ2WpS0xL52uCWooSwShvT4C6ic9LvaXQeHAjJIHoOolJDaAgJKACUYlABKMCgBlGBQAijBoARQguHMHSTRGCwmM3fwXZUqoeCoT3tBmtkS6HAooLWZVQkptDiUgJiJcX/fDoukMZZ9j8WFpYr6WAmnTu9F0pgY/+SUwOcEUIJBCaAEgxJACYZKyEUrX898RdLIz/+0cPBGJUxGa1VCzAlNy8T4R1e1tb0koSKJun93BklBL+gTeVqMUvq9tS38uzOaOpd6woG+nWhmtB23bk7j+7dfrt3nJQolCZMi4RwiyZQOi4WF3zh4qDlzrJIA19CWXnBDinu67ZXV64uUx64P6+reiouX+q1sFsayc3j08K0zVxByIuEYwmmgrXyHhOd6o7QovUE/VI329G5HZ2c7GhXtvTqfPXv6AUtLf1yHFETASSn/TRKe46CMVI5gBY4P7MaJgT0Nk27r1X4uGeLLqfnVbu8qYAiRO6IXc/AV2XENq6DDo/9wl8SuTReiDZ+eyuPVzBcrY7p9OblQQC66w1vhJD+cI3xUiYro6d2GVHcHem3YtJmojjqGjw5BbaCWnyXm3v1Y83NMOAlmEPM+1cPqZGLW+qwJFaFSqkXvSlVc3RWRxo9KcRVBWTc+Ahmz/3vtYZXrE7UHp7GBDOuXNGDj8/J/Xa+l8dUMhzj88AvPeMEyQB+bTDGY5bMI1iqPokbqkRDFRyBD44hEKpRT9+NmMZjRSznOi3DbmfPUwnpKiEMl+GE5jGCSLeeOxO2Yc3MIGrqhv5JtxqOfNqD0cJJ27M9hnWbvWuGbJVCCQQmgBIMSQAkGJYASDEoAJRiUAEowKAGUYFACKMGgBFCCQQmgBIMSQAkGJYASDEoAJRiUAEowKAGUUDfDCNYHNUqkUSPsCaAEgxJACcZfeS7Izttt9E0AAAAASUVORK5CYII=",
                    });
                context.SaveChanges();
            }
        }
    }
}
