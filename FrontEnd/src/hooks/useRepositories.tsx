import { useState, useEffect } from "react";
import { getReposByOrg } from "../api/GitHubAPIHelpers";
import { OrganizationModel } from "../Organization/OrganizationModel";

const useRepositories = (orgName: string) => {
  const [organization, setOrganization] = useState<OrganizationModel>();

  useEffect(() => {
    getReposByOrg(orgName)
      .then(org => setOrganization(org));
  }, []);

  return organization;
};

export default useRepositories;
