using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EasyLearn.Models;
using EasyLearn.Services;

namespace EasyLearn.Controllers;

[Authorize]
public class DoubtChatController : Controller
{
    private readonly IDoubtChatService _doubtService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DoubtChatController(IDoubtChatService doubtService, UserManager<ApplicationUser> userManager)
    {
        _doubtService = doubtService;
        _userManager = userManager;
    }

    [Route("doubts")]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user!);

        if (roles.Contains("Instructor"))
        {
            var doubts = await _doubtService.GetInstructorDoubtsAsync(user!.Id);
            return View("InstructorIndex", doubts);
        }
        else
        {
            var doubts = await _doubtService.GetStudentDoubtsAsync(user!.Id);
            return View("StudentIndex", doubts);
        }
    }

    [Route("doubts/create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Route("doubts/create")]
    public async Task<IActionResult> Create(int courseId, string subject, string message, MessageCategory category = MessageCategory.Doubt)
    {
        var user = await _userManager.GetUserAsync(User);
        await _doubtService.CreateDoubtAsync(user!.Id, courseId, subject, message, category);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("doubts/create")]
    public IActionResult CreateWithCourse(int? courseId)
    {
        ViewBag.CourseId = courseId;
        return View("Create");
    }

    [Route("doubts/{id}")]
    public async Task<IActionResult> Chat(int id)
    {
        var doubt = await _doubtService.GetDoubtByIdAsync(id);
        if (doubt == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        await _doubtService.MarkMessagesAsReadAsync(id, user!.Id);

        return View(doubt);
    }

    [HttpPost]
    [Route("doubts/{id}/send")]
    public async Task<IActionResult> SendMessage(int id, string message)
    {
        var user = await _userManager.GetUserAsync(User);
        await _doubtService.SendMessageAsync(id, user!.Id, message);
        return RedirectToAction(nameof(Chat), new { id });
    }

    [HttpPost]
    [Route("doubts/{id}/resolve")]
    public async Task<IActionResult> Resolve(int id)
    {
        await _doubtService.MarkAsResolvedAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
