using Microsoft.AspNetCore.Mvc;
using webapi.Controllers;

namespace webapi.Controllers
{
    public class TestController
    {
    }
}
public class TestController : ApiController
{
    [HttpGet]
    public int GetAdd(int a, int b)
    {
        return a + b;
    }

    [HttpPost]
    public int PostAdd(int a, int b)
    {
        return a - b;
    }

    [HttpPut]
    public int PutAdd(int a, int b)
    {
        return a * b;
    }

    [HttpDelete]
    public int DeleteAdd(int a, int b)
    {
        return a / b;
    }
}