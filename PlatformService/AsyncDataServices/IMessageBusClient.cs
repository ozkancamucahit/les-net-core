﻿using PlatformService.DTOS;

namespace PlatformService.AsyncDataServices
{
	public interface IMessageBusClient
	{
		void PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO);

	}
}
