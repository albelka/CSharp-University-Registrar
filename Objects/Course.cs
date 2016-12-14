using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Registrar
{
  public class Course
  {
    private int _id;
    private string _name;
    private string _courseNumber;

    public Course(string Name, string CourseNumber, int Id=0)
    {
      _id = Id;
      _name = Name;
      _courseNumber = CourseNumber;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if (!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = (this.GetId() == newCourse.GetId());
        bool nameEquality = (this.GetName() == newCourse.GetName());
        bool courseNumberEquality = (this.GetCourseNumber() == newCourse.GetCourseNumber());

        return (idEquality && nameEquality && courseNumberEquality);
      }
    }

    public override int GetHashCode()
    {
      return _name.GetHashCode();
    }

    public static void Enroll(int courseId, int studentId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO registrations VALUES (@CourseId, @StudentId);", conn);
      cmd.Parameters.AddWithValue("@CourseId", courseId);
      cmd.Parameters.AddWithValue("@StudentId", studentId);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void UnEnroll(int courseId, int studentId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM registrations WHERE course_id = @CourseId, student_id = @StudentId);", conn);
      cmd.Parameters.AddWithValue("@CourseId", courseId);
      cmd.Parameters.AddWithValue("@StudentId", studentId);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Student> GetStudents()
    {
      List<Student> enrolledStudents = new List<Student> {};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses JOIN registrations ON (courses.id = registrations.course_id) JOIN students ON (registrations.student_id = students.id) WHERE courses.id=@CourseId;", conn);

      cmd.Parameters.AddWithValue("CourseId", this.GetId());

      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int studentId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime studentEnrollmentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, studentEnrollmentDate, studentId);
        enrolledStudents.Add(newStudent);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return enrolledStudents;
    }

    public static List<Course> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      List<Course> allCourses = new List<Course> {};
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        string courseCourseNumber = rdr.GetString(2);
        allCourses.Add(new Course(courseName, courseCourseNumber, courseId));
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allCourses;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }


    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses(name, course_number) OUTPUT INSERTED.id VALUES (@CourseName, @CourseCourseNumber)", conn);
      cmd.Parameters.AddWithValue("@CourseName", this.GetName());
      cmd.Parameters.AddWithValue("@CourseCourseNumber", this.GetCourseNumber());
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Course Find(int targetId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id= @Id;", conn);
      cmd.Parameters.AddWithValue("@Id", targetId);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCourseId = 0;
      string foundCourseName = null;
      string foundCourseNumber = null;

      while (rdr.Read())
      {
        foundCourseId = rdr.GetInt32(0);
        foundCourseName = rdr.GetString(1);
        foundCourseNumber = rdr.GetString(2);
      }

      Course foundCourse = new Course(foundCourseName, foundCourseNumber, foundCourseId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCourse;
    }

    public void Update(string propertyToChange, string changeValue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand();
      cmd.Connection = conn;

      if (propertyToChange == "name")
      {
        cmd.CommandText = "UPDATE courses SET name = @Input WHERE id = @Id";
        this.SetName(changeValue);
      }
      else if (propertyToChange == "course_number")
      {
        cmd.CommandText = "UPDATE courses SET course_number = @Input WHERE id = @Id";
        this.SetCourseNumber(changeValue);
      }
      cmd.Parameters.AddWithValue("@Input", changeValue);
      cmd.Parameters.AddWithValue("@Id", this.GetId());

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM courses WHERE id=@CourseId;", conn);
      cmd.Parameters.AddWithValue("@CourseId", this.GetId());

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public string GetCourseNumber()
    {
      return _courseNumber;
    }
    public void SetCourseNumber(string newCourseNumber)
    {
      _courseNumber = newCourseNumber;
    }
  }
}
