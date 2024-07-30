# File Storage MicroService

This project is a microservice for file storage using Cloud S3. It includes functionality to upload and download files with cache support.

## Technologies Used

- .NET Core 6
- Cloud S3
- Swashbuckle (Swagger)
- Microsoft.Extensions.Logging

## Configuration

### Prerequisites

- .NET Core SDK 6.0 or higher
- Cloud account with access to S3
- Visual Studio 2022

### S3 Cloud Configuration

Make sure to configure your S3 Cloud credentials in `appsettings.json`.

```json
{
   "S3": {
      "AccessKey": "YOUR_ACCESS_KEY",
      "SecretKey": "YOUR_SECRET_KEY",
      "RegionEndpoint": "YOUR_S3_ENDPOINT"
   }
}
```

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/filestorage-microservice.git
   cd filestorage-microservice
   ```

2. Open the project in Visual Studio 2022.

3. Restore dependencies:
   ```sh
   dotnet restore
   ```

### Running the Project

1. Configure AWS credentials in `appsettings.json`.

2. Run the project:
   ```sh
   dotnet run
   ```

3. The API will be available at `https://localhost:5001`.

### Usage

#### Upload a File

Endpoint: `POST /api/file/upload`

Request body:
```json
{
  "Key": "file-key",
  "Bucket": "your-bucket-name",
  "File": "file-content",
  "ContentType": "file-content-type"
}
```

#### Download a File

Endpoint: `GET /api/file/download/{bucketName}/{key}`

This endpoint supports a 5-minute cache.

### Project Structure

- `Controllers/FileController.cs`: Controller handling file upload and download requests.
- `Services/S3Service.cs`: Service interacting with Cloud S3.
- `Models/UploadRequestModel.cs`: Model representing the file upload request.
- `Middleware/ApiKeyMiddleware.cs`: Middleware that verifies the API key in requests.

### Security

The project uses an API Key for security. Ensure to include the API Key in the request headers:

```http
ApiKey: YOUR_SECURE_API_KEY
```

### Logging

The project uses `Microsoft.Extensions.Logging` for logging. Logs can be viewed in the console.

### Swagger

Swagger is configured for API documentation and is available in the development environment at `https://localhost:5001/swagger`.

### Contributing

Contributions are welcome. Please follow the steps below to contribute:

1. Fork the project.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Make your changes and commit them (`git commit -am 'Add a new feature'`).
4. Push the changes to your fork (`git push origin feature/your-feature`).
5. Open a Pull Request.

### License

This project is licensed under the MIT License. See the `LICENSE` file for more details.
