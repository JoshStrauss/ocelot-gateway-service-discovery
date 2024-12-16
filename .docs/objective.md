# Objective 

## What?

Introduce a **service registry solution** into the mediSIGHT architecture that fulfils the following requirements:

- **Service registration** – services register themselves on startup to make themselves available to process requests.
- **Service discovery** – provide a mechanism that allows different services to communicate with each other without the need to hardcode addresses between services.
- **Health monitoring** – services that become unhealthy should be removed from the list of services that are available to service requests.
- Integrate the solution with the gateway to allow for traffic to **load balanced** across multiple instances of a service.

## Why?

- Allows for services to be deployed and scaled independently with consuming services being unaware and unaffected by any scale outs/ins.
- Increases the fault tolerance of the solution as requests can be re-routed to healthy services rather than being routed to a service that may or may not be health.
- Makes configuration of individual services easier as services can be referenced by name rather than hardcoded URLs and ports.
- Allows for canary deployments of new service versions.
- Provides a centralised view of the distributed system, making it easier to understand which versions of which services are currently deployed.

## How?

1. Investigate existing out of the box solutions for feasibility.
1. Implement a proof of concept for one or more candidate solutions.
1. Present findings to the relevant stakeholders with a proposal for a solution, detailing the pros and cons of the proposed solution.
1. Implement the chosen solution into mediSIGHT pending release to production.
1. Produce the required documentation to configure and manage the chosen solution.