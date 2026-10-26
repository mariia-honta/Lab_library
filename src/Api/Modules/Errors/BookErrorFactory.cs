using Application.Books.Exceptions;
using Microsoft.AspNetCore.Mvc;
namespace Api.Modules.Errors;

public static class BookErrorFactory
{
    public static ObjectResult ToObjectResult(this BookException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                BookAlreadyExistsException => StatusCodes.Status409Conflict,
                BookNotFoundException => StatusCodes.Status404NotFound,
                BookUnhandledException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Book error handler is not implemented.")
            }
        };
    }
}