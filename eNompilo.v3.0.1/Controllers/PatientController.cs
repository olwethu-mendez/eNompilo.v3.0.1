﻿using eNompilo.v3._0._1.Areas.Identity.Data;
using eNompilo.v3._0._1.Models;
using eNompilo.v3._0._1.Models.SystemUsers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using eNompilo.v3._0._1.Models.ViewModels;

namespace eNompiloCounselling.Controllers
{
    public class PatientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;

        public PatientController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IHttpContextAccessor contextAccessor)
        {
            dbContext = context;
            webHostEnvironment = hostEnvironment;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            IEnumerable<Patient> objList = dbContext.tblPatient;
            return View(objList);
        }

        //public IActionResult EditPatient(int? Id)
        //{
        //    if(Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var obj = dbContext.tblPatient.Find(Id);
        //    if(obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);
        //}

        //public async Task<IActionResult> EditPatient(Patient model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    dbContext.tblPatient.Update(model);
        //    dbContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public IActionResult PatientProfile(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = dbContext.tblPatient.Find(Id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        public IActionResult PatientDashboard(int? Id)
        {
            PatientDashboardViewModel dashboardVM = new PatientDashboardViewModel();
            dashboardVM.PatientFiles = dbContext.tblPatientFile.Find(Id);
            dashboardVM.CounsellingAppointments = dbContext.tblCounsellingAppointment.Find(Id);
            dashboardVM.GeneralAppointments = dbContext.tblGeneralAppointment.Find(Id);
            dashboardVM.VaccinationAppointments = dbContext.tblVaccinationAppointment.Find(Id);
            return View(dashboardVM);
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult AddPersonalDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPersonalDetails(PersonalDetails model)
        {
            if(model.ProfilePictureImageFile != null)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(model.ProfilePictureImageFile.FileName);
                string ext = Path.GetExtension(model.ProfilePictureImageFile.FileName);
                model.ProfilePicture = fileName = fileName + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ext;
                string path = Path.Combine(wwwRootPath + "\\img\\uploads", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.ProfilePictureImageFile.CopyToAsync(fileStream);
                }
            }   
            if (model.PatientId != null && model.Gender != null && model.DOB != null && model.EmergencyPerson != null && model.EmergenyContactNr != null && model.Employed != null && model.Citizenship != null && model.MaritalStatus != null && model.AddressLine1 != null && model.City != null && model.Province != null && model.ZipCode != null)
            {
                dbContext.tblPersonalDetails.Add(model);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("AddMedicalHistory");
            }         
            return View(model);
        }

        public IActionResult EditPersonalDetails(int? Id) //View Not created due to image update issue
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = dbContext.tblPersonalDetails.Find(Id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPersonalDetails(PersonalDetails model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            dbContext.tblPersonalDetails.Update(model);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddMedicalHistory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMedicalHistory(MedicalHistory model)
        {
            //var userId = _userManager.GetUserId(User);
            //var patient = dbContext.tblPatient.SingleOrDefault(c => c.UserId == userId);
            //var patientId = patient.Id;

            //HttpContext.Session.SetInt32("PatientId", patientId);


            //!!!Can we revamp the above code so that when the medical history is submitted (granted we move it tpo inside the ModelState.IsValid) then it retrieves the Medical History's Id and submits it with the Patient's Id to the Patient file. That way, a registered patient with a complete file and history can have a file automatically created for them.!!!
            if (model.PatientId != null && model.PreviousDiagnoses != null && model.PreviousMedication != null && model.GeneralAllergies != null && model.MedicationAllergies != null) //model state was not returning valid.
            {
                dbContext.tblMedicalHistory.Add(model);
                dbContext.SaveChanges();

                var patientId = _contextAccessor.HttpContext.Session.GetInt32("PatientId");

                if (_contextAccessor.HttpContext.Session.GetInt32("PatientId2") == null)
                {
                    patientId = _contextAccessor.HttpContext.Session.GetInt32("PatientId");
                }
                else if (_contextAccessor.HttpContext.Session.GetInt32("PatientId") == null)
                {
                    patientId = _contextAccessor.HttpContext.Session.GetInt32("PatientId2");
                }

                var medicalHistory = dbContext.tblMedicalHistory.SingleOrDefault(c => c.PatientId == patientId);
                var medicalHistoryId = medicalHistory.Id;
                HttpContext.Session.SetInt32("MedicalHistoryId", medicalHistoryId);

                int? truePatientId = _contextAccessor.HttpContext.Session.GetInt32("PatientId");

                if (_contextAccessor.HttpContext.Session.GetInt32("PatientId2") == null)
                {
                    truePatientId = _contextAccessor.HttpContext.Session.GetInt32("PatientId");
                }
                else if (_contextAccessor.HttpContext.Session.GetInt32("PatientId") == null)
                {
                    truePatientId = _contextAccessor.HttpContext.Session.GetInt32("PatientId2");
                }

                var patientFile = new PatientFile
                {
                    PatientId = truePatientId,
                    MedicalHistoryId = medicalHistoryId,
                };
                dbContext.tblPatientFile.Add(patientFile);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }    
            return View(model);
        }

        public IActionResult EditMedicalHistory(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var obj = dbContext.tblMedicalHistory.Find(Id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMedicalHistory(MedicalHistory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            dbContext.tblMedicalHistory.Update(model);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
