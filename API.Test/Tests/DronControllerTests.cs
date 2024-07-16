using API.Data.Entidades.Seguridad;
using API.Domain.Interfaces.Seguridad;
using API.Domain.Services.Seguridad;
using API.Domain.Validators.Seguridad;

namespace API.Test.Tests
{
    [TestClass]
    public class APIControllerTests : BasicTest<Usuario, UsuarioValidator>
    {
        private readonly IUsuarioService _servicioUsuario;

        public APIControllerTests()
        {
            _servicioUsuario = new UsuarioService(_repositories, null);
        }

        #region GetAll Tests

        //[TestMethod, Priority(1), TestCategory("List")]
        //public void GetAll_ReturnOk()
        //{
        //    // Arrange
        //    var controller = new UsuarioController(_mapper, _servicioUsuario);

        //    // Act
        //    var response = controller.ObtenerTodos().Result;

        //    // Assert
        //    Assert.AreEqual(StatusCodes.Status200OK, (response as OkObjectResult)?.StatusCode);
        //}

        //[TestMethod, Priority(1), TestCategory("List")]
        //public void GetAll_ReturnAPIList()
        //{
        //    // Arrange
        //    var controller = new UsuarioController(_mapper, _servicioUsuario);

        //    // Act
        //    ResponseDto? responseDto = (controller.ObtenerTodos().Result as OkObjectResult)?.Value as ResponseDto;
        //    var response = responseDto?.Result;

        //    // Assert
        //    Assert.IsInstanceOfType(response, typeof(List<UsuarioDto>));
        //}

        //[TestMethod, Priority(1), TestCategory("List")]
        //public void GetAll_ReturnCorrectlyNumberOfElements()
        //{
        //    // Arrange
        //    var controller = new UsuarioController(_mapper, _servicioUsuario);

        //    // Act
        //    ResponseDto? responseDto = (controller.ObtenerTodos().Result as OkObjectResult)?.Value as ResponseDto;
        //    var response = responseDto?.Result;

        //    // Assert
        //    Assert.AreEqual(10, (response as List<UsuarioDto>)?.Count);
        //}
        #endregion
    }
}
