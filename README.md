Technology used: .Net 8, Entity Framework 8.0.10, .NetCore Web API 8.0.10, SQLServer

PROJECT

Build a Patient Registration Service 

Implement a web API simulates the capture and management of patient registration information. This is a headless service, meaning, there is no user interface component. Patient registration information includes the following: 

Patient Demographics: a patient has a name, unique medical record number, age, gender, and a set of contacts.  

Admitting Diagnosis: a patient can be admitted with a breast, lung, prostate, or unspecified cancer diagnosis.  

Attending Physician: breast and lung cancer patients are assigned to Dr. Susan Jones; all other patients are assigned to Dr. Ben Smith. 

Department: patients seeing Dr. Susan Jones are assigned to department J; all other patients are assigned to department S. 

Functional Requirements 

New patients are registered daily within the cancer clinic. Existing patient information can be modified by clinical staff, but once a patient has been assigned an admitting diagnosis, they cannot be removed from the system. 

Non-Functional Requirements 

Patient registration information must be accessible by external applications, and the web service is to be used to replace a legacy patient registration application.

GetById Request:
https://localhost:7257/api/Patients/{id}

GetAllPatients: Used JWT token for Authentication
https://localhost:7257/api/

AddPatient:
https://localhost:7257/api/Patients/
Body: 
{
  "name": "Test",
  "medicalRecordNumber": "50085",
  "age": 4527,
  "gender": "male",
  "phone": "7787812369",
  "email": "test@exampl.com",
  "emergencyContact": "9958884762"
}

Patch Update: Used JWT token for Authentication
https://localhost:7257/api/Patients/{id}
Body:
[
   
     { "op": "replace", "path": "/age", "value": "42" },
     { "op": "replace", "path": "/diagnosisName", "value": "" }
]

Delete: Used JWT token for Authentication
https://localhost:7257/api/Patients/{id}

To generate JWT token:
Post SignUp:
https://localhost:7257/api/Account/signup
Body:
{
  "email": "doctor@gmail.com",
  "password": "Doctor@123",
  "confirmPassword": "Doctor@123"
}

Post SignIn:
https://localhost:7257/api/Account/signin
Body:
{
  "email": "ClinicAdmin@gmail.com",
  "password": "ClinicAdmin@123"
}

