import { OrganizationModel } from "../Organization/OrganizationModel";
import { RepositoryModel } from "../Repository/RepositoryModel";


const url = "http://localhost:51044/api";
const headersSettings = { 'Content-Type': 'application/json'};

export async function getReposByOrg(orgName: string) {
    const relativeAddress = `org/${orgName}/repos`;
    const requestOptions = {
        method: 'GET', headers: headersSettings
    };

    const response = await fetch(`${url}/${relativeAddress}`, requestOptions)
    const repos = await response.json() as RepositoryModel[];
    const org = parseOrganization(repos);
    if (org)
        return org;
}

function parseOrganization(repos: RepositoryModel[]) {
    if (repos?.length > 0) {
        const orgName = repos[0].fullName.split('/')[0];
        const org = new OrganizationModel(orgName, repos);
        return org;
    }
}

export async function setRepoProtectionState(orgName: string, repoName:string, isProtected:boolean){
    const relativeAddress = `org/${orgName}/repo/${repoName}?isProtected=${isProtected}`;
    const requestOptions = {
        method: 'POST', headers: headersSettings
    };

    const response = await fetch(`${url}/${relativeAddress}`, requestOptions);
    if(response.status == 200){
        return true;
    }
    console.log(`SetRepoProtectionState return not ok: ${response}`);
    return false;
}