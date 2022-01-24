# Recruitment Tool Sample with ASP.Net Core 6.0 Web API

## Candidate

### Schema:
{
  "firstName": "FirstName >= 2 && FirstName  <= 50",
  "lastName": "LastName >= 2 && LastName <= 50",
  "email": "Email >= 3 && Email <= 320",
  "bio": "Bio >= 10 && Bio <= 2000",
  "birthday": "between 18 and 99 age",
  "recruiter": {
    "lastName": "LastName >= 2 && LastName <= 50"",
    "email": "Email >= 3 && Email <= 320",
    "country": "Country >= 2 && Country <= 90"
  },
  "skills": [
    {
      "name": "SkillsCount >= 1"
    }
  ]
}

| Method | Request URL                           |
| -------|:-------------------------------------:|
| POST   | {host}/api/v{version}/candidates      |
| GET    | {host}/api/v{version}/candidates      |
| GET    | {host}/api/v{version}/candidates/{id} |
| DELETE | {host}/api/v{version}/candidates/{id} |
| PUT    | {host}/api/v{version}/candidates/{id} |
| PATCH  | {host}/api/v{version}/candidates/{id} |

* *Method PUT replaces the hole object. All required fields should be sent*
* *Method PATCH allows partial update.*

Queries: 
•	Page={1}
•	PageSize={10} (max size by default 50)
•	Query={PropertyName}_{PropertyValue}
•	OrderBy= {PropertyName}_{Asc/Desc}

Example: https://localhost:7065/api/v1/candidates?Page=1&PageSize=10&Query=FirstName_Ivan&OrderBy=LastName_Descending
Response:
{
  "value": [
    {
      "id": "1642122e-b82e-4c2d-bc3f-6dae1d4274bc",
      "firstName": "Ivan",
      "lastName": "Enchev",
      "email": "ivan@fake.de",
      "bio": " Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
      "birthday": "1987-01-21",
      "recruiter": {
        "id": "dbcc03f4-ae94-48fd-87a4-26b18d0e5324",
        "lastName": "TheBestOne",
        "email": "best@hr.de",
        "country": "Deutschland",
        "experienceLevel": 4
      },
      "skills": [
        {
          "id": 1,
          "name": "C#"
        }
      ],
      "links": [
        {
          "href": "https://localhost:7065/api/v1/candidates/1642122e-b82e-4c2d-bc3f-6dae1d4274bc",
          "rel": "self",
          "method": "GET"
        },
        {
          "href": "https://localhost:7065/api/v1/candidates",
          "rel": "create_candidate",
          "method": "POST"
        },
        {
          "href": "https://localhost:7065/api/v1/candidates/1642122e-b82e-4c2d-bc3f-6dae1d4274bc",
          "rel": "update_candidate",
          "method": "PUT"
        },
        {
          "href": "https://localhost:7065/api/v1/candidates/1642122e-b82e-4c2d-bc3f-6dae1d4274bc",
          "rel": "patch_candidate",
          "method": "PATCH"
        },
        {
          "href": "https://localhost:7065/api/v1/candidates/1642122e-b82e-4c2d-bc3f-6dae1d4274bc",
          "rel": "delete_candidate",
          "method": "DELETE"
        }
      ]
    },
                ...............
  "links": [
    {
      "href": "https://localhost:7065/api/v1/candidates?page=2&pagesize=10",
      "rel": "self",
      "method": "GET"
    },
    {
      "href": "https://localhost:7065/api/v1/candidates?page=1&pagesize=10",
      "rel": "first",
      "method": "GET"
    },
    {
      "href": "https://localhost:7065/api/v1/candidates?page=4&pagesize=10",
      "rel": "last",
      "method": "GET"
    },
    {
      "href": "https://localhost:7065/api/v1/candidates?page=3&pagesize=10",
      "rel": "next",
      "method": "GET"
    },
    {
      "href": "https://localhost:7065/api/v1/candidates?page=1&pagesize=10",
      "rel": "previous",
      "method": "GET"
    },
    {
      "href": "https://localhost:7065/api/v1/candidates",
      "rel": "create_candidate",
      "method": "POST"
    }
  ]
}

## Interviews
When we create a job, an interview with a suitable candidate will be created automatically. A suitable candidate is one who has at least one skill required by the job. 

| Method | Request URL                       |
| -------|:---------------------------------:|
| GET    | {host}/api/v{version}/intervies   |

Queries: 
•	Page={1}
•	PageSize={10} (max size by default 50)
•	Query=Date_{SameDate}
•	OrderBy= Date_{}

Example:  https://localhost:7065/api/v1/ intervies?Page=1&PageSize=10&Query= Date _2022-01-28T01:30:00&OrderBy=Date_Desc
{
  "value": [
    {
      "candidate": {
        "id": "1642122e-b82e-4c2d-bc3f-6dae1d4274bc",
        "firstName": "Ivan1",
        "lastName": "Enchev1",
        "email": "1van@abv.bg",
        "bio": "stringstri",
        "birthday": "1987-01-21T09:33:52.289",
        "recruiter": {
          "id": "dbcc03f4-ae94-48fd-87a4-26b18d0e5324",
          "lastName": "Kratun",
          "email": "kratun@dd.de",
          "country": "Deutschland",
          "experienceLevel": 4
        },
        "skills": [
          {
            "id": 1,
            "name": "C#"
          }
        ]
      },
      "job": {
        "id": 4,
        "title": "The job",
        "description": "The best job as C# Junior Web Dev in the city!",
        "salary": 3500,
        "interviewsCount": 3,
        "skills": [
          {
            "id": 1,
            "name": "C#"
          }
        ]
      },
      "date": "2022-01-28T20:00:01.2998225"
    },
  ],
                 .............
  "links": [
	       PAGANATION LINKS
  ]
}

## Recruiters

| Method | Request URL                       |
| -------|:---------------------------------:|
| GET    | {host}/api/v{version}/recruiters  |

Queries: 
•	Page={1}
•	PageSize={10} (max size by default 50)
•	Query=ExperienceLevel_{int >= 1 }
•	OrderBy= {LastName/Email/Country/ExperienceLevel }_{Asc/Desc}

Example: https://localhost:7065/api/v1/recruiters?Page=1&Query=ExperienceLevel_4&OrderBy=LastName_Desc&PageSize=10

## Skills

| Method | Request URL                         |
| -------|:-----------------------------------:|
| GET    | {host}/api/v{version}/skills/{id}   |
| GET    | {host}/api/v{version}/skills/active |

* */active returns only skills for which we have candidates at this moment*

Example: https://localhost:7065/api/v1/skills/active?Page=1&OrderBy=Name_Desc&PageSize=10

## Jobs

### Schema:
{
  "title": "Title >= 2 && Title <= 100",
  "description": "Description >= 20 && Description <= 2000",
  "salary": 0,
  "skills": [
    {
      "name": "SkillsCount >= 1"
    }
  ]
}

| Method | Request URL                      |
| -------|:--------------------------------:|
| POST   | {host}/api/v{version}/jobs       |
| GET    | {host}/api/v{version}/jobs       |
| DELETE | {host}/api/v{version}/jobs/{id}  |

* *delete all interviews for this job and free up the slots of the recruiters responsible for it.*

Example: https://localhost:7065/api/v1/jobs?Page=1&Query=Title_Developer&OrderBy=Salary_Desc&PageSize=10
{
  "value": [
    {
      "id": 5,
      "title": "Developer",
      "description": "stringstringstringst",
      "salary": 10220,
      "interviewsCount": 0,
      "skills": [
        {
          "id": 1,
          "name": "C#"
        }
      ],
      "links": [
	             PAGANATION LINKS
  ]
}
