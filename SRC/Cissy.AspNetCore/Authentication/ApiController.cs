using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Cissy;
using Cissy.Serialization;

namespace Cissy.Authentication
{

    public abstract class ApiController : Controller
    {
        public IActionResult JsonPack<T>(JsonBase<T> json, bool CamelCase = false)
        {
            var str = json.ModelToJson(CamelCase);
            var result = new ContentResult()
            {
                Content = str,
                ContentType = "application/json"
            };
            //return Json(json);
            return result;
        }
        public IActionResult ErrMessage(int State, string Message = "", string Data = "", bool CamelCase = false)
        {
            if (State == 0)
                return JsonPack(new Err(-1, "错误码不能为0", "错误码不能为0,0代表成功"), CamelCase);
            return JsonPack(new Err(State, Data, Message), CamelCase);
        }
        public IActionResult Success<T>(T Data, string Message = "", bool CamelCase = false)
        {
            return JsonPack(new Success<T>(Data, Message), CamelCase);
        }
        //public IActionResult Success(string Message, bool CamelCase = false)
        //{
        //    return JsonPack(new Success<string>(string.Empty, Message), CamelCase);
        //}
    }
}
