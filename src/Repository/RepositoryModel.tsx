export class RepositoryModel {
    id: number;
    name: string;
    fullName: string;
    isPrivate: boolean;
    isProtected: boolean;
    avatarUrl: string = "Octocat2.png"
    /**
     * ctor for init new git hub repository
     */
    constructor(id: number, name: string, fullName:string, isPrivate: boolean, isProtected: boolean) {
        this.id = id;
        this.name = name;
        this.fullName = fullName;
        this.isPrivate = isPrivate;
        this.isProtected = isProtected;
    }
}