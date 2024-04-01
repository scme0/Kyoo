// Kyoo - A portable and vast media library solution.
// Copyright (c) Kyoo.
//
// See AUTHORS.md and LICENSE file in the project root for full license information.
//
// Kyoo is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// Kyoo is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Kyoo. If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using AspNetCore.Proxy;
using Kyoo.Abstractions.Models.Utils;
using Kyoo.Core.Api;
using Kyoo.Core.Controllers;
using Kyoo.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Kyoo.Core.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureMvc(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();

		services
			.AddMvcCore(options =>
			{
				options.Filters.Add<ExceptionFilter>();
				options.ModelBinderProviders.Insert(0, new SortBinder.Provider());
				options.ModelBinderProviders.Insert(0, new IncludeBinder.Provider());
				options.ModelBinderProviders.Insert(0, new FilterBinder.Provider());
			})
			.AddJsonOptions(x =>
			{
				x.JsonSerializerOptions.TypeInfoResolver = new JsonKindResolver()
				{
					Modifiers = { IncludeBinder.HandleLoadableFields }
				};
				x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				x.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			})
			.AddDataAnnotations()
			.AddControllersAsServices()
			.AddApiExplorer()
			.ConfigureApiBehaviorOptions(options =>
			{
				options.SuppressMapClientErrors = true;
				options.InvalidModelStateResponseFactory = ctx =>
				{
					string[] errors = ctx
						.ModelState.SelectMany(x => x.Value!.Errors)
						.Select(x => x.ErrorMessage)
						.ToArray();
					return new BadRequestObjectResult(new RequestError(errors));
				};
			});

		services.Configure<RouteOptions>(x =>
		{
			x.ConstraintMap.Add("id", typeof(IdentifierRouteConstraint));
		});

		services.AddResponseCompression(x =>
		{
			x.EnableForHttps = true;
		});

		services.AddProxies();
		services.AddHttpClient();
	}

	// Stollen from https://stackoverflow.com/questions/44934511/does-net-core-dependency-injection-support-lazyt
	public static IServiceCollection AllowLazy(this IServiceCollection services)
	{
		ServiceDescriptor lastRegistration = services.Last();
		Type serviceType = lastRegistration.ServiceType;

		// The constructor for Lazy<T> expects a Func<T> which is hard to create dynamically.
		Type lazyServiceType = typeof(Lazy<>).MakeGenericType(serviceType);

		// Create a typed MethodInfo for `serviceProvider.GetRequiredService<T>`,
		// where T has been resolved to the required ServiceType
		System.Reflection.MethodInfo? getRequiredServiceMethod = typeof(ServiceProviderServiceExtensions).GetMethod(
			nameof(ServiceProviderServiceExtensions.GetRequiredService),
			1,
			[typeof(IServiceProvider)]
		);

		System.Reflection.MethodInfo? getRequiredServiceMethodTyped = getRequiredServiceMethod?.MakeGenericMethod(
			serviceType
		);

		// Now create a lambda expression equivalent to:
		//
		//     serviceProvider => serviceProvider.GetRequiredService<T>();
		//
		ParameterExpression parameterExpr = Expression.Parameter(typeof(IServiceProvider), "serviceLocator");
		LambdaExpression lambda = Expression.Lambda(
			Expression.Call(null, getRequiredServiceMethodTyped!, parameterExpr),
			parameterExpr
		);

		Delegate lambdaCompiled = lambda.Compile();

		services.Add(
			new ServiceDescriptor(
				lazyServiceType,
				serviceProvider =>
					Activator.CreateInstance(
						lazyServiceType,
						lambdaCompiled.DynamicInvoke(serviceProvider)
					)!,
				lastRegistration.Lifetime
			)
		);

		return services;
	}
}
