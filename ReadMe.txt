

1.Open Blank Solution which is name "SocialMediaApp".
2.Open Class Library (.Core) Project which is name "SocialMediaApp.DomainLayer".
	2.1. Open "Enums" Folder. 
        2.1.1. In this Folder, Open "Status.cs" class.		 
	2.2. Open "Entities" folder. Create entities in Interface and Concrete folder.
        2.2.1. Open Concrete folder.
            2.2.1.1. AppUser.cs
            2.2.1.2. AppRole.cs
            2.2.1.3. Follow.cs
            2.2.1.4. Like.cs
            2.2.1.5. Mention.cs
            2.2.1.6. Share.cs
            2.2.1.7. Tweet.cs
        2.2.2. Open Interface folder.
            2.2.2.1. IBase.cs
            2.2.2.2. IBaseEntity.cs
		 P.S : I use "Asp .Net Core Identity" class for users operations.In this context, AppUserRole and AppUser class inhereted from Identity Class. For this,  install Microsoft.AspCore.Identity(5.0.2) package.
                - install Microsoft.Extensions.Identity.Stores(5.0.2)
	2.3. Open "Repository" folder. In this folder, I create methods for CRUD operation properly asynchronous programming .
        2.3.1 Open BaseRepository Folder
                2.3.1.1 IBaseRepository.cs
        2.3.2 Open EntityTypRepository Folder.
                2.3.2.1. IAppUserRepository
                2.3.2.2. IFollowRepository
                2.3.2.3. ILikeRepository
                2.3.2.4. IMentionRepository
                2.3.2.5. IShareRepository
                2.3.2.6. ITweetRepository
	2.4. Open "UnitOfWork" folder.
		 2.4.1. IUnitOfWork.cs
3.Open Class Library (.Core) Project which is name "SocialMediaApp.Infrastructure".
    P.S : Add reference "SocialMediaApp.DomainLayer".
              and install -Microsoft.AspNetCore.Identity.EntityFrameworkCore(5.0.3)
                          -Microsoft.EntityFrameworkCore(5.0.3)
                          -Microsoft.EntityFrameworkCore.SqlServer(5.0.3)
                          -Microsoft.EntityFrameworkCore.Tools(5.0.3) package.
    3.1. Open "Mapping" folder.For mapping operation.
        3.1.1 Open Abstract Folder.
                3.1.1.1 BaseMap.cs
        3.2.2 Open Concrete Folder.
                3.2.2.1. AppRoleMap
                3.2.2.2. AppUserMap
                3.2.2.3. FollowMap
                3.2.2.4. LikeMap
                3.2.2.5. MentionMap
                3.2.2.6. ShareMap
                3.2.2.7. TweetMap
    3.2. Open "Context" folder. For connection with DB.
        3.2.1. ApplicationDbContext.cs
        public class ApplicationDbContext:IdentityDbContext<AppUser, AppRole,int> 
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<Tweet> Tweets { get; set; }
            public DbSet<Mention> Mentions { get; set; }
            public DbSet<Share> Shares { get; set; }
            public DbSet<Like> Likes { get; set; }
            public DbSet<Follow> Follows { get; set; }
            public DbSet<AppUser> AppUsers { get; set; }
            public DbSet<AppRole> AppRoles { get; set; }

            //Onmodelcreatin methodu eager loading için kullanılır lazy loading için configuration methodu kullanılır.

            protected override void OnModelCreating(ModelBuilder builder)
            {
            builder.ApplyConfiguration(new TweetMap());
            builder.ApplyConfiguration(new MentionMap());
            builder.ApplyConfiguration(new ShareMap());
            builder.ApplyConfiguration(new LikeMap());
            builder.ApplyConfiguration(new FollowMap());
            builder.ApplyConfiguration(new AppUserMap());
            builder.ApplyConfiguration(new AppRoleMap());
            base.OnModelCreating(builder);
            }
        }
    3.3. Open "Repository" folder. In this folder, implement repository which is in domainlayer  
        3.3.1 Open BaseRepository Folder
                2.3.1.1 BaseRepository.cs
        public abstract class BaseRepository<T> : IRepository<T> where T : class, IBaseEntity
      {
        private readonly ApplicationDbContext _applicationDbContext;
        protected DbSet<T> _table;

        public BaseRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
            this._table = _applicationDbContext.Set<T>();
        }


        public async Task Add(T entity) => await _table.AddAsync(entity);
        public async Task<bool> Any(Expression<Func<T, bool>> expression) => await _table.AnyAsync(expression);
        public void Delete(T entity) => _table.Remove(entity); // delete methodu asenkron olmaz.    
        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression) => await _table.Where(expression).FirstOrDefaultAsync();
        public async Task<List<T>> Get(Expression<Func<T, bool>> expression) => await _table.Where(expression).ToListAsync();
        public async Task<List<T>> GetAll() => await _table.ToListAsync();
        public async Task<T> GetById(int id) => await _table.FindAsync(id);



        //AsNoTracking; Entity Framework tarafından uygulamaların performansını optimize etmemize yardımcı olmak için geliştirilmiş bir fonksiyondur. İşlevsel olarak veritabanından sorgu neticesinde elde edilen nesnelerin takip mekanizması ilgili fonksiyon tarafından kırılarak, sistem tarafından izlenmelerine son verilmesini sağlamakta ve böylece tüm verisel varlıkların ekstradan işlenme yahut lüzumsuz depolanma süreçlerine maliyet ayrılmamaktadır.
        //AsNoTracking fonksiyonu ile takibi kırılmış tüm nesneler doğal olarak güncelleme durumlarında “SaveChanges” fonksiyonundan etkilenmeyecektirler.
        public async Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = _table;
            if (disableTracking) query = query.AsNoTracking(); // önemli bir olgu eğer getirdiklerimiz crud işlemlerinde kullanılırsa açık bırakırız. burada filtrede kullanılacağı için notracking kullanıcaz. 
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);
            if (orderby != null) return await orderby(query).Select(selector).FirstOrDefaultAsync();
            else return await query.Select(selector).FirstOrDefaultAsync();           
        }

        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true, int pageIndex = 1, int pageSize = 3)
        {
            IQueryable<T> query = _table;
            if (disableTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);
            if (expression != null) query = query.Where(expression);
            if (orderby != null) return await orderby(query).Select(selector).Skip((pageIndex - 1) * pageIndex).Take(pageSize).ToListAsync();
            else return await query.Select(selector).ToListAsync();
        }

        public void Update(T entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
        }
      }
        3.3.2 Open EntityTypRepository Folder.
                3.3.2.1. AppUserRepository
                public AppUserRepository(ApplicationDbContext applicationDbContext):base (applicationDbContext){ }
                3.3.2.2. FollowRepository
                3.3.2.3. LikeRepository
                3.3.2.4. MentionRepository
                3.3.2.5. ShareRepository
                3.3.2.6. TweetRepository
    3.4. Open "UnitOfWork" folder.
		 3.4.1. IUnitOfWork.cs
    //unitOfWork u kullanma amacımız ; bankamatik örneğindeki gibi her seferinde db ye gönderip işlem yapmasını önlemek ve en son onaydan sonra db ile bağlantı kurmak. Burada da Repositorylerin bağlantısını tek elden yürütmek amacıyla UnitOfWork kullanılıyor. 
    public class UnitOfWork:IUnitOfWork //unit of work u implement ettik. 
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext ?? throw new ArgumentNullException("database can not be null");
            //ctor injection yönetminde hata çıkarsa. ?? karar mekanizması else benzeri db dönsün yada cümlecik fırlatsın
        }

        private IAppUserRepository _appUserRepository;
        public IAppUserRepository AppUserRepository
        {
            //get { return _appUserRepository ?? (_appUserRepository = new AppUserRepository(_applicationDbContext)); }

            get
            {
                if (_appUserRepository == null) _appUserRepository = new AppUserRepository(_applicationDbContext);
                return _appUserRepository; 
            }
        }

        private IFollowRepository _followRepository;
        public IFollowRepository FollowRepository { get => _followRepository ?? (_followRepository = new FollowRepository(_applicationDbContext)); }


        private ILikeRepository _likeRepository;
        public ILikeRepository LikeRepository { get => _likeRepository ?? (_likeRepository = new LikeRepository(_applicationDbContext)); }


        private IMentionRepository _mentionRepository;
        public IMentionRepository MentionRepository { get => _mentionRepository ?? (_mentionRepository = new MentionRepository(_applicationDbContext)); }


        private IShareRepository _shareRepository;
        public IShareRepository ShareRepository { get => _shareRepository ?? (_shareRepository = new ShareRepository(_applicationDbContext)); }

        private ITweetRepository _tweetRepository;
        public ITweetRepository TweetRepository { get => _tweetRepository ?? (_tweetRepository = new TweetRepository(_applicationDbContext)); }

        public async Task Commit() => await _applicationDbContext.SaveChangesAsync();
        public async Task ExecuteSqlRaw(string sql, params object[] paramters) => await _applicationDbContext.Database.ExecuteSqlRawAsync(sql, paramters);
        private bool isDisposing = false;

        //????? bunlar neden kullanıldı UNİT OF WORK için önemli bir içerik.... 
        //  İşlemler bittiğinden GC kullanılması için yazıldı. Nesne yönetimi için kullanılan bir yapıdır. 
        public async ValueTask DisposeAsync()
        {
            if (!isDisposing)
            {
                isDisposing = true;
                await DisposeAsync(true); //db.Dispose yazabildirdik ama burada farklı bir kullanım.
                GC.SuppressFinalize(this);
            }
        }
        private async Task DisposeAsync(bool disposing)
        {
            if (disposing) await _applicationDbContext.DisposeAsync();

        }
     }
 4.Open Class Library (.Core) Project which is name "SocialMediaApp.Application".
    P.S : Add reference -"SocialMediaApp.DomainLayer"
                        - "SocialMediaApp.Infrastructure".
              and install -Autofac(6.1.0)
                          -AutoMapper(10.1.1)
                          -AutoMapper.Extensions.Microsoft.DependencyInjection(8.1.1)
                          -FluentValidation.AspNetCore(9.5.0)
                          -Microsoft.AspNetCore.Http.Features(5.0.3)
                          -Microsoft.AspNetCore.Identity(2.2.0)
                          -Microsoft.Extensions.DependencyInjection(5.0.1)
                          -SixLabors.ImageSharp(1.0.2)         

    4.1. Open "Services" folder.
        4.1.1 Open Interface Folder.
                4.1.1.1. AppUserService.cs
                4.1.1.2. FollowServic.cs
                4.1.1.3. LikeService.cs
                4.1.1.4. MentionService.cs
                4.1.1.5. TweetService.cs
        4.1.2 Open Concrete Folder.
                4.1.2.1. IAppUserService.cs
                4.1.2.2. IFollowServic.cs
                4.1.2.3. ILikeService.cs
                4.1.2.4. IMentionService.cs
                4.1.2.5. ITweetService.cs
                
    4.2. Open "Model" folder. For DTO and VM.
        4.2.1. Open DTOs Folder.
                4.2.1.1. AddMentionDTO.cs
                4.2.1.2. AddTweetDTO.cs
                4.2.1.3. EditProfileDTO.cs
                4.2.1.4. FollowDTO.cs
                4.2.1.5. LikeDTO.cs
                4.2.1.6. LoginDTO.cs
                4.2.1.7. ProfileSummaryDTO.cs
                4.2.1.8. RegisterDTO.cs
        4.2.2. VM Folder.
                4.2.2.1. FollowListVM.cs
                4.2.2.2. TimeLineVM.cs
    4.3. Open "Mapper" folder.
        4.3.1. Mapping.cs.
        public Mapping()
        {
            CreateMap<AppUser, RegisterDTO>().ReverseMap();
            CreateMap<AppUser, LoginDTO>().ReverseMap();
            CreateMap<AppUser, EditProfileDTO>().ReverseMap();
            CreateMap<AppUser, ProfileSummaryDTO>().ReverseMap();

            CreateMap<Follow, FollowDTO>().ReverseMap();
            CreateMap<Like, LikeDTO>().ReverseMap();

            CreateMap<Mention, AddMentionDTO>().ReverseMap();

        }
    4.4. Open IoC folder.
        4.4.1. AutoFacContainer.cs
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppUserService>().As<IAppUserService>().InstancePerLifetimeScope();
            builder.RegisterType<FollowService>().As<IFollowService>().InstancePerLifetimeScope();
            builder.RegisterType<LikeService>().As<ILikeService>().InstancePerLifetimeScope();
            builder.RegisterType<TweetServices>().As<ITweetService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();


            builder.RegisterType<LoginValidation>().As<IValidator<LoginDTO>>().InstancePerLifetimeScope();
            builder.RegisterType<RegisterValidation>().As<IValidator<RegisterDTO>>().InstancePerLifetimeScope();
            builder.RegisterType<TweetValidation>().As<IValidator<AddTweetDTO>>().InstancePerLifetimeScope();
        }
    4.5. Open Extensions folder.
        4.5.1. ClaimsPrincipalExtensions.cs
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserEmail(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Email);

        public static int GetUserId(this ClaimsPrincipal principal) => Convert.ToInt32(principal.FindFirstValue(ClaimTypes.NameIdentifier));

        public static string GetUserName(this ClaimsPrincipal principal) => principal.FindFirstValue(ClaimTypes.Name);

        public static bool IsCurrentUser(this ClaimsPrincipal principal, string id)
        {
            var currentUserId = GetUserId(principal).ToString();
            return string.Equals(currentUserId, id);          
        }
    }
    4.6. Open Validation folder.
        4.6.1. LoginValidation.cs
         public LoginValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Enter a username");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter a username");
        }
        4.6.2. RegisterValidation.cs
          public RegisterValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Enter e mail Adress").EmailAddress().WithMessage("Please type into a valid e mail adress");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter e mail Adress");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x=>x.Password).WithMessage("Password do not macth");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Password do not macth");
        }
        4.6.3. TweetValidation.cs
         public class TweetValidation:AbstractValidator<AddTweetDTO>
        {
        public TweetValidation() => RuleFor(x => x.Text).NotEmpty().WithMessage("Can not't be empty").MaximumLength(256).WithMessage("Must be less then 256 character");
        }

5.Open Asp .Net Core Web Application Project which is name "SocialMediaApp.Presentation".
    P.S : Add reference -"SocialMediaApp.Application"
                        - "SocialMediaApp.Infrastructure".
              and install -Autofac.Extensions.DependencyInjection(7.1.0)
                          -Microsoft.EntityFrameworkCore(5.0.3)
                          -Microsoft.EntityFrameworkCore.Design(5.0.3)
                          -Microsoft.EntityFrameworkCore.SqlServer(5.0.3)            
                          -Microsoft.Extensions.DependencyInjection(5.0.1)
                          -Microsoft.VisualStudio.Web.CodeGeneration.Design(3.1.5)
    5.1. Startup.cs 
        5.1.1. Classes that cause dependencies are registered in the "Configure Service ()" method.
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews().AddFluentValidation();
            services.AddAutoMapper(typeof(Mapping));

            //"AddIdentity" sınıfı için Microsoft.AspNetCore.Identity paketi indirilir.
            services.AddIdentity<AppUser, AppRole>(x => {
                x.SignIn.RequireConfirmedAccount = false;
                x.SignIn.RequireConfirmedEmail = false;
                x.SignIn.RequireConfirmedPhoneNumber = false;
                x.User.RequireUniqueEmail = false;
                x.Password.RequiredLength = 3;
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireNonAlphanumeric = false; })
    5.2. appsettings.json => Necessary action is taken for connection with DB.
        "AllowedHosts": "*",
         "ConnectionStrings": {
         "DefaultConnection": "Server=DESKTOP-J1BE3LG;Database=SocialMedia;Trusted_Connection=True;" }
    5.3. Program.cs =>Implement "ıoc container" that is in "Application Layer" for thirdt-part IOC container.
         public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterModule(new AutoFacContainer());
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    5.4. "Migration" operations are done.
    5.5. Add Controller in Controllers folder.
        5.5.1. AccountController.cs
    public class AccountController : Controller
    {
        private readonly IAppUserService _userService;

        public AccountController(IAppUserService appUserService)
        {
            _userService = appUserService;
        }

        #region Registration
        public IActionResult Register()
        {
            ///?????
            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");
            return View();         
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(registerDTO);
                //?? neden succeeded oldu
                if (result.Succeeded) return RedirectToAction("Index", "Home");

                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Description);                
            }

            return View(registerDTO);
        
        }

        #endregion

        #region Login
        //?? neden null oldu
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");

            //readme ye eklenecek gitbook yapılacak viewdata viewbag tempdata
            ViewData["ReturnUrl"] = returnUrl;
            return View();      
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model, string returnUrl) 
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LogIn(model);
                //?? RedirectToLocal nedemek gitbook basicknowledge
                if (result.Succeeded) return RedirectToLocal(returnUrl);

                ModelState.AddModelError(String.Empty, "Invalid login attempt....");
              
            }
            return View();

        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            else return RedirectToAction(nameof(HomeController.Index), "Home");

        }

        #endregion

        #region Logout
        [HttpPost]

        public async Task<IActionResult> LogOut() 
        {
            await _userService.LogOut();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        
        }

        #endregion

        #region EditProfile
        public async Task<IActionResult> EditProfile(string userName)
        {
            if (userName == User.Identity.Name)
            {
                var user = await _userService.GetById(User.GetUserId());

                if (user == null) return NotFound();
                return View(user);
            }
            else return RedirectToAction(nameof(HomeController.Index), "Home");            
        }

        [HttpPost]
        //??? Ifrom File nedir gitbook
        public async Task<IActionResult> EditProfile(EditProfileDTO model, IFormFile file) 
        {
            await _userService.EditUser(model);
            return RedirectToAction(nameof(HomeController.Index), "home");           
        }

        #endregion
      
    }
        5.5.2. FollowController.cs
        5.5.3. HomeController.cs
        5.5.4. ProfileController.cs
        5.5.5. TweetController.cs
    P.S : I add neccesary methods and use Asenkron programing in Controller.cs.And then add View for this methods. 
    5.6. Add "ViewComponent" folder in Models folder.
        5.6.1. AddTweet.cs
        public IViewComponentResult Invoke()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(claim.Value);
            var tweet = new AddTweetDTO();
            tweet.AppUserId = userId;
            return View(tweet);        
        }
        5.6.2. FollowUser.cs
        5.6.3. ProfileSummary.cs
    P.S : Necessary procedures are done (Components Partial View etc.) for the Front End part. 
        
        



