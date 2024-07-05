using Asp.Versioning;
using Catalog.API.Mediator.Commands.CategoryValue;
using Catalog.API.Mediator.Queries.CategoryValue;
using Catalog.DTO.Category;
using Catalog.DTO.CategoryValue;
using DomainHelper.Exceptions;
using InfrastructureHelper.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

/// <summary>
/// API-Controller for category value operations
/// </summary>
/// <param name="logger">The logger for this controller</param>
/// <param name="mediator">The mediator to delegate requests to</param>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CategoryValueController(ILogger<CategoryValueController> logger, IMediator mediator) : ControllerBase
{
    #region Get Data
    /// <summary>
    /// Get all category values
    /// </summary>
    /// <returns>A list of all category values</returns>
    /// <response code="200">Returns list of all category values</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpGet()]
    [Route("GetAllCategoryValues")]
    [ProducesResponseType(typeof(IEnumerable<CategoryValueDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CategoryValueDTO>>> GetAllCategoryValues()
    {
        try
        {
            logger.LogInformation("GetAllCategories called");

            var allCategoryValuesResult = await mediator.Send(new GetAllCategoryValuesQuery());

            return Ok(allCategoryValuesResult);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Get all category values for specified category
    /// </summary>
    /// <param name="id">The unique identifier for the category</param>
    /// <returns>A list of all category values for specified category</returns>
    /// <response code="200">Returns list of all category values for specified category</response>
    /// <response code="401">Not authorized</response>
    /// <response code="404">The category for specified unique identifier was not found</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpGet()]
    [Route("GetAllCategoryValuesForCategory/{id}")]
    [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryDTO>> GetAllCategoryValuesForCategory(int id)
    {
        try
        {
            logger.LogInformation("GetAllCategoryValuesForCategory called");

            var categoryValuesForCategory = await mediator.Send(new GetCategoryValuesForCategoryQuery()
            {
                Id = id
            });

            return Ok(categoryValuesForCategory);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Get a category value
    /// </summary>
    /// <param name="id">Unique identifier for the category value</param>
    /// <returns>The requested category value</returns>
    /// <response code="200">Returns category value with requested unique identifier</response>
    /// <response code="401">Not authorized</response>
    /// <response code="404">The category value for specified unique identifier was not found</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryValueDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryValueDTO>> Get(int id)
    {
        try
        {
            logger.LogInformation("GetCategoryValue called");

            var categoryValueResult = await mediator.Send(new GetCategoryValueQuery()
            {
                Id = id
            });

            return Ok(categoryValueResult);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region CRUD
    /// <summary>
    /// Create a new category value
    /// </summary>
    /// <param name="model">The model with data for the category value</param>
    /// <returns>The newly created category value</returns>
    /// <response code="200">Returns created category value</response>
    /// <response code="400">Not all necassary values in request model are given</response>
    /// <response code="401">Not authorized</response>
    /// <response code="409">Conflict with unique keys. e.g. Name, Description</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryValueDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryValueDTO>> Create([FromBody] CategoryValueCreateDTO model)
    {
        try
        {
            logger.LogInformation("CreateCategoryValue called");

            var createCategoryResult = await mediator.Send(new MtrMakeNewCategoryValueCmd() { InputData = model });

            return Ok(createCategoryResult);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.WrongParameter)
        {
            return BadRequest();
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch (DataDuplicatedValueException)
        {
            return Conflict();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Change an existing category value
    /// </summary>
    /// <param name="id">The unique identifier for the category value</param>
    /// <param name="model">The model with changed data for the category value</param>
    /// <returns>The changed category value</returns>
    /// <response code="200">Returns updated category value</response>
    /// <response code="400">Not all necassary values in request model are given</response>
    /// <response code="401">Not authorized</response>
    /// <response code="404">The category with specified unique identifier was not found</response>
    /// <response code="409">Conflict with unique keys. e.g. Name, Description</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoryValueDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryValueDTO>> Update(long id, [FromBody] CategoryValueUpdateDTO model)
    {
        try
        {
            logger.LogInformation("UpdateCategoryValue called");

            var updateCategoryResult = await mediator.Send(new MtrUpdateCategoryValueCmd() { InputData = model });

            return Ok(updateCategoryResult);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.WrongParameter)
        {
            return BadRequest();
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch (DataDuplicatedValueException)
        {
            return Conflict();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Delete an existing category value
    /// </summary>
    /// <param name="id">The unique identifier for the category value</param>
    /// <returns>The HTTP-Status-Code for the delete operation</returns>
    /// <response code="200">No return value</response>
    /// <response code="401">Not authorized</response>
    /// <response code="404">The category value with specified unique identifier was not found</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            logger.LogInformation("DeleteCategory called");

            await mediator.Send(new MtrDeleteCategoryValueCmd() { Id = id });

            return Ok();
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.WrongParameter)
        {
            return BadRequest();
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch (DataDuplicatedValueException)
        {
            return Conflict();
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Validation
    /// <summary>
    /// Check if a german category value name already exists in the database
    /// </summary>
    /// <param name="model">The model with data to check</param>
    /// <returns>True = Already exists / False = Not exists</returns>
    /// <response code="200">True = Already exists / False = Not exists</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost]
    [Route("CheckGermanCategoryValueNameAlreadyExists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> CheckGermanCategoryValueNameAlreadyExists([FromBody] CheckCategoryValueNameExistsDTO model)
    {
        try
        {
            logger.LogInformation("CheckGermanCategoryValueNameAlreadyExists called");

            var result = await mediator.Send(new GermanCategoryValueNameExistsQuery() { InputData = model });

            return Ok(result);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.WrongParameter)
        {
            return BadRequest();
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch
        {
            throw;
        }
    }

    /// <summary>
    /// Check if an english category value name already exists in the database
    /// </summary>
    /// <param name="model">The model with data to check</param>
    /// <returns>True = Already exists / False = Not exists</returns>
    /// <response code="200">True = Already exists / False = Not exists</response>
    /// <response code="401">Not authorized</response>
    /// <response code="500">Internal server error. Please contact support if so.</response>
    [HttpPost]
    [Route("CheckEnglishCategoryValueNameAlreadyExists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> CheckEnglishCategoryValueNameAlreadyExists([FromBody] CheckCategoryValueNameExistsDTO model)
    {
        try
        {
            logger.LogInformation("CheckEnglishCategoryValueNameAlreadyExists called");

            var result = await mediator.Send(new EnglishCategoryValueNameExistsQuery() { InputData = model });

            return Ok(result);
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.WrongParameter)
        {
            return BadRequest();
        }
        catch (DomainException ex) when (ex.ExceptionType == DomainExceptionType.NoDataFound)
        {
            return NotFound();
        }
        catch
        {
            throw;
        }
    }
    #endregion
}
