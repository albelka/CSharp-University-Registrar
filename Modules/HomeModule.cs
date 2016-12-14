
using Nancy;
using System;
using System.Collections.Generic;
using Nancy.ViewEngines.Razor;


namespace Registrar
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["/index.cshtml"];
      };

      Get["/courses"] = _ => {
        List<Course> allCourses = Course.GetAll();
        return View["all_courses.cshtml", allCourses];
      };
      Post["/courses"] = _ => {
        int StudentId = int.Parse(Request.Form["student"]);
        int CourseId = int.Parse(Request.Form["course"]);
        Course.Enroll(CourseId, StudentId);
        Course newCourse = Course.Find(CourseId);
        return View["course.cshtml", newCourse];
      };
      Get["/unenroll"] = _ => {
        return View["/registration.cshtml"];
      };
      Post["/unenroll"] = _ => {
        int StudentId = int.Parse(Request.Form["student"]);
        int CourseId = int.Parse(Request.Form["course"]);
        Course.UnEnroll(CourseId, StudentId);
        return View["/registration.cshtml"];
      };

      Get["/courses/new"] = _ => {
        return View["new_course_form.cshtml"];
      };

      Post["/courses/new"] = _ => {
        string courseName = Request.Form["course-name"];
        string courseNumber = Request.Form["course-number"];
        Course newCourse = new Course(courseName, courseNumber);
        newCourse.Save();
        List<Course> allCourses = Course.GetAll();
        return View["all_courses.cshtml", allCourses];
      };

      Get["/courses/{id}"] = parameters => {
        Course foundCourse = Course.Find(parameters.id);
        return View["course.cshtml", foundCourse];
      };

      Get["/students"] = _ => {
        List<Student> allStudents = Student.GetAll();
        return View["all_students.cshtml", allStudents];
      };

      Get["/students/new"] = _ => {
        return View["new_student_form.cshtml"];
      };

      Post["/students/new"] = _ => {
        string studentName = Request.Form["student-name"];
        DateTime enrollmentDate = (DateTime) Request.Form["enrollment-date"];
        Student newStudent = new Student(studentName, enrollmentDate);
        newStudent.Save();
        List<Student> allStudents = Student.GetAll();
        return View["all_students.cshtml", allStudents];
      };

      Get["/students/{id}"] = parameters => {
        Student foundStudent = Student.Find(parameters.id);
        return View["student.cshtml", foundStudent];
      };

      Get["/register"] = _ => {
        List<Student> allStudents = Student.GetAll();
        List<Course> allCourses = Course.GetAll();
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("students", allStudents);
        model.Add("courses", allCourses);
        return View["registration.cshtml", model];
      };
    }
  }
}
