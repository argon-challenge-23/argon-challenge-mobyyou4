import { RepositoryModel } from "../Repository/RepositoryModel";

export class OrganizationModel{
    name: string;
    repositories: RepositoryModel[];
    /**
     *ctor for Git Hub Organization
     */
    constructor(name:string, repositories:RepositoryModel[]) {
        this.name = name;
        this.repositories = repositories;
    }
}