# Dotnet Rate Limiter

This project was developed to test the rate limiter in dotnet applications, how to configure the policies.

## What is that?

A rate limiter in the context of APIs is a vital mechanism used to control the volume of incoming requests from clients. It enforces predefined limits on the number of requests a client can make within a specific time period, preventing abuse, ensuring equitable resource distribution, and maintaining API stability. When a client attempts a request, the rate limiter assesses whether the client's request count has surpassed the allocated limit for the given time window. If the limit is exceeded, the API might respond with an error message, safeguarding the server from potential overload due to excessive requests. Rate limiting supports fair usage, deters malicious behavior, and guarantees consistent performance.

## Fixed Window Limiter

A Fixed Window Rate Limiter is a type of rate limiting strategy often used in software applications to control the rate at which certain operations or requests are allowed to occur. In the context of .NET or any programming environment, this type of rate limiter ensures that a certain number of operations can be performed within a fixed time window.

### Implementation

To implement apply this configurations:

In `Program.cs`:

```c#
using System.Threading.RateLimiting;
using Dotnet.Rate.Limiter.Api.Context;

// ...

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10)
            }
        ));
});

// ...

app.UseRateLimiter();
```

In `controller`:

```c#
using Microsoft.AspNetCore.RateLimiting;

// ...

[HttpGet]
[EnableRateLimiting("fixed")]
public async Task<IActionResult> GetAll() { }
```

## Token Bucket Limiter

The token bucket limiter is similar to the sliding window limiter, but rather than adding back the requests taken from the expired segment, a fixed number of tokens are added each replenishment period. The tokens added each segment can't increase the available tokens to a number higher than the token bucket limit.

### Implementation

To implement apply this configurations:

In `Program.cs`:

```c#
using System.Threading.RateLimiting;
using Dotnet.Rate.Limiter.Api.Context;

// ...

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("token", httpContext =>
        RateLimitPartition.GetTokenBucketLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 5,
                ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                TokensPerPeriod = 2
            }
        ));
});

// ...

app.UseRateLimiter();
```

In `controller`:

```c#
using Microsoft.AspNetCore.RateLimiting;

// ...

[HttpGet]
[EnableRateLimiting("token")]
public async Task<IActionResult> GetAll() { }
```

## Concurrency Limiter

The concurrency limiter limits the number concurrent requests. Each request reduces the concurrency limit by one. When a request completes, the limit is increased by one. Unlike the other requests limiters that limit the total number of requests for a specified period, the concurrency limiter limits only the number of concurrent requests and doesn't cap the number of requests in a time period.

### Implementation

To implement apply this configurations:

In `Program.cs`:

```c#
using System.Threading.RateLimiting;
using Dotnet.Rate.Limiter.Api.Context;

// ...

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("concurrency", httpContext =>
        RateLimitPartition.GetConcurrencyLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new ConcurrencyLimiterOptions
            {
                PermitLimit = 2
            }
        ));
});

// ...

app.UseRateLimiter();
```

In `controller`:

```c#
using Microsoft.AspNetCore.RateLimiting;

// ...

[HttpGet]
[EnableRateLimiting("concurrency")]
public async Task<IActionResult> GetAll() { }
```

## Sliding Window Limiter

A sliding window limiter manages data or event influx by monitoring a moving time window. It restricts excessive input by counting events within the window and applying limits. As time advances, the window slides, discarding outdated events. This technique maintains system stability and prevents overload in scenarios like APIs and networks, ensuring controlled resource consumption.

### Implementation

To implement apply this configurations:

In `Program.cs`:

```c#
using System.Threading.RateLimiting;
using Dotnet.Rate.Limiter.Api.Context;

// ...

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("sliding", httpContext =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new SlidingWindowRateLimiterOptions
            {
                Window = TimeSpan.FromSeconds(15),
                SegmentsPerWindow = 3,
                PermitLimit = 15
            }
        ));
});

// ...

app.UseRateLimiter();
```

In `controller`:

```c#
using Microsoft.AspNetCore.RateLimiting;

// ...

[HttpGet]
[EnableRateLimiting("sliding")]
public async Task<IActionResult> GetAll() { }
```

## Test

To run this project you need docker installed on your machine, see the docker documentation [here](https://www.docker.com/).

Having all the resources installed, run the command in a terminal from the root folder of the project and wait some seconds to build project image and download the resources:
`docker-compose up -d`

In terminal show this:

```console
[+] Running 2/2
 ✔ Network dotnet-rate-limiter_default  Created                0.8s
 ✔ Container rate_limiter_app           Started                1.2s
```

After this, access the link below:

- Swagger project [click here](http://localhost:5000/swagger)

### Stop Application

To stop, run: `docker-compose down`
