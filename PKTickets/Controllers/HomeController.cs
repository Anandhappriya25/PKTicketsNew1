using PKTickets.Repository;
using PKTickets.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PKTickets.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PKTickets.Interfaces;
using PKTickets.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
                                    
namespace PKTickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPayTypeRepository _payTypeRepository;
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly IShowRepository _seatRepository;
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWebHostEnvironment WebHostEnvironment;


        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository,
            IPayTypeRepository payTypeRepository, IMovieRepository movieRepository,
            ITheaterRepository theaterRepository, IScreenRepository screenRepository,
            IShowRepository seatRepository, IShowTimeRepository showTimeRepository,
            IReservationRepository reservationRepository, IRoleRepository roleRepository,
            IWebHostEnvironment _webHostEnvironment)
        {
            _logger = logger;
            _userRepository = userRepository;
            _payTypeRepository = payTypeRepository;
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _screenRepository = screenRepository;
            _seatRepository = seatRepository;
            _showTimeRepository = showTimeRepository;
            _reservationRepository = reservationRepository;
            _roleRepository = roleRepository;
            WebHostEnvironment = _webHostEnvironment;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var user = _userRepository.GetLoginDetail(login.EmailId, login.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier , user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                     new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(ClaimTypes.Role, user.RoleName),
                    new Claim(ClaimTypes.Email,user.EmailId)

                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Redirect(login.ReturnUrl == null ? "/Home/Index" : login.ReturnUrl);
            }
            else
                ViewBag.Message = "Invalid Credential";
            return View(login);
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }


        public IActionResult Profile(int id)
        {
            User user = _userRepository.UserById(id);
            Role role = _userRepository.GetRoleById(user.RoleId);
            UserDTO userDTO = new UserDTO();
            userDTO.UserId = user.UserId;
            userDTO.UserName = user.UserName;
            userDTO.PhoneNumber = user.PhoneNumber;
            userDTO.Location = user.Location;
            userDTO.EmailId = user.EmailId;
            userDTO.Password = user.Password;
            userDTO.RoleName = role.RoleName;
            return View(userDTO);

        }


        public IActionResult UsersList()
        {
            var usersList = _userRepository.UsersList();
            return View(usersList);
        }

        public IActionResult CreateUser()
        {
            UserDTO userDTO = new UserDTO();
            if (User.Identity.IsAuthenticated)
            {
                var role = User.Identity.GetClaimRole();
                if (role == "Admin")
                {
                    userDTO.RoleIds = _userRepository.GetAllRole().Select(a => new SelectListItem
                    {
                        Text = a.RoleName,
                        Value = a.RoleId.ToString()
                    }).ToList();
                    userDTO.RoleIds.Insert(0, new SelectListItem { Text = "Select Role", Value = "" });
                    userDTO.Role = true;
                }
            }
            else
            {
                userDTO.RoleIds = _userRepository.GetAllRole().Where(x => x.RoleName == "User").Select(a => new SelectListItem
                {
                    Text = a.RoleName,
                    Value = a.RoleId.ToString()
                }).ToList();
                userDTO.Role = false;
            }
            return View(userDTO);
        }

        [HttpPost]
        public IActionResult Save(UserDTO user)
        {
            if (user.UserId > 0)
            {
                return Json(_userRepository.UpdateUser(user));
            }
            else
            {
                return Json(_userRepository.CreateUser(user));
            }
        }


        public IActionResult TheatersList()
        {
            var theatersList = _theaterRepository.GetTheaters();
            return View(theatersList);
        }

        public IActionResult CreateTheater()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Theater(Theater theater)
        {
            if (theater.TheaterId > 0)
            {
                return Json(_theaterRepository.UpdateTheater(theater));
            }
            else
            {
                return Json(_theaterRepository.CreateTheater(theater));
            }
        }


        public IActionResult ShowtimesList()
        {
            var showTimesList = _showTimeRepository.GetAllShowTimes();
            return View(showTimesList);
        }

        public IActionResult PayTypesList()
        {
            var paytypesList = _payTypeRepository.GetAllPayTypes();
            return View(paytypesList);
        }
        public IActionResult MoviesList()
        {
            var moviesList = _movieRepository.GetAllMovies();
            return View(moviesList);
        }
        public IActionResult AddMovie(Movie movie)
        {
            return View(movie);
        }
        public async Task<IActionResult> Add(Movie movie)
        {

            var uploadDirectory = "Css/Image/";
            string location = "~wwwroot/Css/Image/";
            var uploadPath = Path.Combine(WebHostEnvironment.WebRootPath, uploadDirectory);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(movie.CoverPhoto.FileName);
            var imagePath = Path.Combine(uploadPath, fileName);
            await movie.CoverPhoto.CopyToAsync(new FileStream(imagePath, FileMode.Create));
            movie.ImagePath = fileName;
            if (movie.MovieId > 0)
            {
                return Json(_movieRepository.UpdateMovie(movie));
            }
            else
            {
                return Json(_movieRepository.CreateMovie(movie));
            }
        }
        public IActionResult MovieDetails(int id)
        { 
            return View(_movieRepository.MovieById(id));
        }


        [HttpPost]
        public IActionResult SaveMovie(Movie movie)
        {
            if (movie.MovieId > 0)
            {
                return Json(_movieRepository.UpdateMovie(movie));
            }
            else
            {
                return Json(_movieRepository.CreateMovie(movie));
            }
        }
    }
}