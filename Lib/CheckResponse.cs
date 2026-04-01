using ItineraryOperations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace ItineraryOperations.Lib
{
    public static class CheckResponse
    {
        public static ActionResult<T> CheckNotFoundObject<T>(
            this ControllerBase controller,
            T? checkedObject, 
            string message = "Такого объекта нет")
        {
            if (checkedObject == null)
            {
                return controller.NotFound(new APIError { Message = message });
            }
            else
            {
                return controller.Ok(checkedObject);
            }
        }

        public static ActionResult<T[]> CheckNotFoundArray<T>(
            this ControllerBase controller,
            T[] checkedObject,
            string message = "Такого объекта нет")
        {
            if (checkedObject.Length == 0)
            {
                return controller.NotFound(new APIError { Message = message });
            }
            else
            {
                return controller.Ok(checkedObject);
            }
        }

        public static ActionResult<List<T>> CheckNotFoundList<T>(
            this ControllerBase controller,
            List<T> checkedObject,
            string message = "Такого объекта нет")
        {
            if (checkedObject.Count == 0)
            {
                return controller.NotFound(new APIError { Message = message });
            }
            else
            {
                return controller.Ok(checkedObject);
            }
        }
    }
}
