# What is Service discovery 

Service discovery is the process of automatically detecting and registering the availability and location of services within a distributed system. It is typically used in modern software architectures such as microservices and cloud-native applications, where services are often ephemeral and may be frequently created, updated, or destroyed.

Service discovery enables applications to dynamically locate and communicate with each other, without requiring manual configuration or hard-coding of network addresses. Instead, services register themselves with a central registry, which other services can query to discover the location and availability of the desired service.

One of the primary benefits of service discovery is increased flexibility and scalability. As services are added, removed, or scaled up or down, the service discovery system can dynamically update the registry, ensuring that applications can always find the services they need. This can help to reduce downtime, improve fault tolerance, and increase overall system performance.

## What is consul?

Consul is a software first released in 2014 by HashiCorp.
It is a service mesh solution that provides full featured control plane with DNS-based service discovery and distributed key-value storage, segmentation and configuration and can be used individually or together to build a full service mesh.

## Why choose Consul?

Consul is a good choice when you need to deliver an on-premises solution that doesn't require your potential customers to have existing infrastructure and a cooperative IT provider. Consul is a lightweight single executable, has no dependencies, and as such can easily be built into your middleware solution. When Consul is your solution for discovering, checking, and maintaining your microservices.

1. 'Load balancing': Service discovery can be used to balance traffic across multiple instances of a service, improving performance and resiliency.
1. 'Service composition': Services can be dynamically composed and recomposed to create new applications and workflows on the fly.
1. 'High availability': Service discovery can be used to ensure that services remain available even in the event of a failure or outage.
1. 'Scaling': Service discovery can be used to dynamically scale services up or down based on demand, without requiring manual intervention.

## Consul Vs Eureka

- Simple setup
- Health checks
- KV
- command line application
- UI
- Better client library support and bigger use in community following 
-spring boot eureka vs GO

