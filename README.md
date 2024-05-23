# Voting

This project contains an API and UI for the Voting Application

## Set up your development environment

### Dependencies

This project requires you to have the following installed on your machine.

- Docker
- npm
- dotnet core cli

### Notes for Windows

If you have issues getting Docker for windows running, checkout these docs:

- https://docs.docker.com/docker-for-windows/troubleshoot/
- https://docs.docker.com/docker-for-windows/troubleshoot/#virtualization

### Setup

1. Clone this repository `git clone https://github.com/sahilmehta89/voting.git`

### Starting the project

If you are using OSX or a linux based operating system, you can use the Makefile to run `make` in the terminal. This will spin up a PostgreSQL docker container, start the .NET api and start the angular frontend.

If you are not using a linux/unix based operating system (ex: Windows/DOS), you should be able to use the following commands to start the app.

- Navigate to [`API\Voting.API`] folder
- Run `docker-compose up -d` in powershell or git bash. This will start a PostgreSQL database using docker.
- Run `dotnet restore` inside the `API` directory. This will install the C# dependencies needed to run the api.
- Then `dotnet run --project Voting.API.csproj` inside the `API` directory. This will start the api server.
- Inside another git bash or powershell directory, go to the `UI` directory and run `npm install`.
- After above command is complete, run `npm run start` in the `UI` directory.

You will know everything is running correctly when you go to http://localhost:4200 in your browser, and see a voting portal.

## Development

We recommend below development tools:
- Visual Studio Code for Frontend (UI) development
- Visual Studio for Backend developmentt

However, you can use whatever code editor you would like. To help, here are some good Visual Studio Code extensions we recommend.

This Voting App has the below features:

- Shows the list of Candidates.
- Shows the list of Voters.
- User can create a new Candidate by adding the Candidate Name and clicking on Submit button in the Candidates page.
- User can create a new Voter by adding the Voter Name and clicking on Submit button in the Voters page.
- Voter can vote for a candidate in the Cast Vote page.


If you get stuck getting your development environment setup, reach out to er.sahilmehta89@gmail.com
