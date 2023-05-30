﻿using Devops.Deploy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devops.Deploy.Clients
{
    public static class ClientExtensions
    {
        public static List<IRelease> Limit(this List<IRelease> Releases, int NumberOfReleases)
        {
            NumberOfReleases = NumberOfReleases < 0 || NumberOfReleases > Releases.Count() ? Releases.Count() : NumberOfReleases;
            return Releases.OrderByDescending(releases=>releases.DeploymentOrCreated).Take(NumberOfReleases).ToList();
        }

        public static List<IDeployment> Limit(this List<IDeployment> Deployments, int NumberOfReleases)
        {
            var ReleaseIds = Deployments.OrderByDescending(deployment => deployment.DeployedAt).Select(deployment => deployment.ReleaseId).Distinct();
            NumberOfReleases = NumberOfReleases < 0 || NumberOfReleases > ReleaseIds.Count() ? ReleaseIds.Count() : NumberOfReleases;

            return Deployments.Where(deployment => ReleaseIds.Take(NumberOfReleases).Contains(deployment.ReleaseId)).ToList();
        }


    }
}
