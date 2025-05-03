namespace Movie_Web.Middlewares
{
    public class CheckAdminMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckAdminMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.ToString().Trim().ToLower();

            if (path.StartsWith("/admin"))
            {
                string result = context.User.FindFirst("IsAdmin")?.Value ?? "";

                if (string.IsNullOrEmpty(result) || bool.Parse(result) == false)
                {
                    //context.Request.Path = "/home/AccessDenied";
                    context.Response.Redirect(location: "/home/AccessDenied");
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }

        }
    }
}
