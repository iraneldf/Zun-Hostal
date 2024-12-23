﻿using API.Application.Dtos.Comunes;
using API.Domain.Exceptions;
using FluentValidation;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Filters;

public class ExceptionManagerFilter : IExceptionFilter
{
    private static readonly ILog _logger = LogManager.GetLogger("");
    private readonly IWebHostEnvironment _environment;

    public ExceptionManagerFilter(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public void OnException(ExceptionContext context)
    {
        var text = "Ha ocurrido un error en el sistema.";

        if (context.Exception is CustomException exception)
        {
            text = exception.Message;

            context.Result = new BadRequestObjectResult(new ResponseDto
                { Status = exception.Status, ErrorMessage = text });
        }
        else
        {
            switch (context.Exception.GetType().Name)
            {
                case nameof(ValidationException):
                    context.Result = new BadRequestObjectResult(new ResponseDto
                    {
                        Status = StatusCodes.Status400BadRequest,
                        ErrorMessage = context.Exception.Message.Replace("Severity: Error", "")
                            .Replace("Validation failed", "Error en los datos insertados")
                    });
                    break;
                case nameof(DbUpdateException):
                    if (context.Exception.InnerException?.Message.StartsWith(
                            "The DELETE statement conflicted with the REFERENCE constraint") ?? false)
                    {
                        text = "Este elemento no puede ser eliminado debido a que otro depende de el";

                        context.Result = new BadRequestObjectResult(new ResponseDto
                            { Status = StatusCodes.Status400BadRequest, ErrorMessage = text });
                    }
                    else
                    {
                        context.Result = new BadRequestObjectResult(new ResponseDto
                            { Status = StatusCodes.Status400BadRequest, ErrorMessage = text });
                    }

                    break;
                default:
                    context.Result = new BadRequestObjectResult(new ResponseDto
                        { Status = StatusCodes.Status400BadRequest, ErrorMessage = text });
                    break;
            }
        }

        //Guardando el log en BD
        _logger.ErrorFormat(context.Exception.ToString());
    }
}