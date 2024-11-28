# ELibrary

The ELibrary project is a web application built using .NET Core MVC, Tw-Elements, and TailwindCSS for efficient library management.

![Borrowing Page](https://drive.usercontent.google.com/download?id=1wpTlkjHZAtbpiXksJRfeOvo0DLe8RxsT)

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Features](#features)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Installation

### Prerequisites

Make sure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/en-us/download) (v8 or higher)
- [Node.js](https://nodejs.org) (v18 or higher)
- [MySQL](https://mysql.com)
- [ViteJS](https://vitejs.dev)

### Steps to install

1. Clone the repository:
   ```bash
   git clone https://github.com/memiljamel/project-elibrary.git
   ```

2. Navigate to the project directory:
   ```bash
   cd project-elibrary
   ```

3. Install dependencies.
    - For .NET Core projects:
      ```bash
      dotnet restore
      ```
    - For Node.js projects:
      ```bash
      npm install
      ```

4. Configure the `appsettings.json` file to set up your MySQL database connection.

5. Create & Run the database migrations:
   ```bash
    dotnet ef migrations add "InitialCreate"
    dotnet ef database update
   ```

6. Start the development server.
    - For .NET Core projects:
      ```bash
      dotnet watch run
      ```
    - For Node.js projects:
      ```bash
      npm run dev
      ```

## Usage

After completing the installation steps and the server is running, open your browser and navigate to [http://localhost:5000](http://localhost:5000).

## Features

- **Authentication:** Provides login functionalities for administrators/staffs.
- **Dashboard:** Display a greeting message.
- **Manage Borrowings:** Enables administrators/staffs to view, create, edit, and delete borrowing information.
- **Manage Books:** Enables administrators/staffs to view, create, edit, and delete book information.
- **Manage Authors:** Enables administrators/staffs to view, create, edit, and delete author information.
- **Manage Members:** Enables administrators/staffs to view, create, edit, and delete member information.
- **Manage Staffs:** Enables administrators to view, create, edit, and delete staff information.
- **Search Functionality:** Enables searching for specific records.
- **Edit Profile:** Enables administrators/staff to update their profile information.

## Contributing

Feel free to contribute to this project by submitting pull requests or reporting issues.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE.md) file for details.

## Contact

- **Email:** [memiljamel@gmail.com](mailto:memiljamel@gmail.com)
- **LinkedIn:** [linkedin.com/in/memiljamel](https://linkedin.com/in/memiljamel)
- **GitHub:** [github.com/memiljamel](https://github.com/memiljamel)
