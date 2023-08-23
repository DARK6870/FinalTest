using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace Final
{
    public static class EndpointBaseAsync
    {
		public static class WithRequest<TRequest1, TRequest2>
		{
			public abstract class WithResult<TResponse> : EndpointBase
			{
				public abstract Task<TResponse> HandleAsync(
					TRequest1 request1,
					TRequest2 request2,
					CancellationToken cancellationToken = default
				);
			}

			public abstract class WithoutResult : EndpointBase
			{
				public abstract Task HandleAsync(
					TRequest1 request1,
					TRequest2 request2,
					CancellationToken cancellationToken = default
				);
			}

			public abstract class WithActionResult<TResponse> : EndpointBase
			{
				public abstract Task<ActionResult<TResponse>> HandleAsync(
					TRequest1 request1,
					TRequest2 request2,
					CancellationToken cancellationToken = default
				);
			}

			public abstract class WithActionResult : EndpointBase
			{
				public abstract Task<ActionResult> HandleAsync(
					TRequest1 request1,
					TRequest2 request2,
					CancellationToken cancellationToken = default
				);
			}

			public abstract class WithAsyncEnumerableResult<T> : EndpointBase
			{
				public abstract IAsyncEnumerable<T> HandleAsync(
					TRequest1 request1,
					TRequest2 request2,
					CancellationToken cancellationToken = default
				);
			}
		}


		public static class WithRequest<TRequest>
        {
            public abstract class WithResult<TResponse> : EndpointBase
            {
                public abstract Task<TResponse> HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }
            public abstract class WithoutResult : EndpointBase
            {
                public abstract Task HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithActionResult<TResponse> : EndpointBase
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithActionResult : EndpointBase
            {
                public abstract Task<ActionResult> HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }
            public abstract class WithAsyncEnumerableResult<T> : EndpointBase
            {
                public abstract IAsyncEnumerable<T> HandleAsync(
                  TRequest request,
                  CancellationToken cancellationToken = default
                );
            }
        }

        public static class WithoutRequest
        {
            public abstract class WithResult<TResponse> : EndpointBase
            {
                public abstract Task<TResponse> HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithoutResult : EndpointBase
            {
                public abstract Task HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithActionResult<TResponse> : EndpointBase
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithActionResult : EndpointBase
            {
                public abstract Task<ActionResult> HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithAsyncEnumerableResult<T> : EndpointBase
            {
                public abstract IAsyncEnumerable<T> HandleAsync(
                  CancellationToken cancellationToken = default
                );
            }
        }

     
    }
}
