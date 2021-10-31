
# Meter Reading

Meter reading is an API to process a CSV file containing meter readings from various account holders. The meter readings are read from the csv, validated and persisted in the data store.

## Usage

Run the following command to build and start the server. Server will be available at **https://localhost:5001**.

```
dotnet run --project source\api
```
Use postman to post a csv file containing meter readings to the following endpoint. Ensure that the name of the key in form data is **"file"**. 
**https://localhost:5001/MeterReading/meter-reading-uploads** 

Response will contain the following
- Number of records successfully processed from the provided csv meter readings. 
- Validation errors for one or more meter readings.

An example 
```json
{
    "numberOfReadingsImported": 4,
    "validationErrors": [
        "Meter reading [4534 - 11/05/2019 9:24] has invalid value []. The meter readings must be 5 digit positive number.",
        "No matching account for the reading [67761 - 10/05/2019 9:24]",
        "Meter reading [1239 - 17/05/2019 9:24] already exists"
    ]
}
```

## Source Structure

The project structure is based on the clean [architecture principals](https://jasontaylor.dev/clean-architecture-getting-started/) to achieve clear separation of concerns.

| Project         | Description |
| ------------- |:-------------:|
| MeterReading.Api  | The API project exposing the endpoint to post a csv file |
| MeterReading.Domain | The project containing the domain entities. These are POCO classes representing the data models.     |
| MeterReading.Application  | The core business logic layer to process CSV data, validate and populate the readings into data store. |
| MeterReading.Infrastructure| The project encapsulating the data storage concerns      |

## Validations

* Meter reading values must be numerical and have five digits (NNNNN)
* Meter reading which do not match with an existing account will be ignored.
* Duplicate (account id and meter read time) meter readings will be ignored.

## TBD

1. User interface to facilitate uploading of CSV file.
2. User interface to support CRUD operation on account and meter reading.

## Platform\Technologies
1. Asp .Net 5.0 
2. Entity Framework Core.
3. nUnit.

## License
[GPL-3.0 License](https://www.gnu.org/licenses/gpl-3.0.en.html)
