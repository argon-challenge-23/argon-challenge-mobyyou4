import { RepositoryModel } from "./RepositoryModel";
import "./repository.css";
import { SetRepoProtectionState } from "../helpers/GitHubAPIHelpers";
import { useState } from "react";

interface repositoryModelProps {
    orgName: string,
    repo: RepositoryModel;
    isProtected: boolean;
}

async function HandleProtectionStateChanged(props: repositoryModelProps, setProtection:any) {
    const success = await SetRepoProtectionState(props.orgName, props.repo.name, !props.repo.isProtected);
    if (success) {
        setProtection(!props.repo.isProtected);
        props.repo.isProtected = !props.repo.isProtected;
        return;
    }
    console.log(`HandleProtectionStateChanged didn't succeeded for: org: ${props.orgName} and repo:${props.repo.name}`)
}

function Repository(props: repositoryModelProps) {
    const [protection, setProtection] = useState<boolean>(props.repo.isProtected);

    return (
        <div className="container">
            <div className="icon-container">
                <img src="Octocat.png" width="61" height="49" />
            </div>
            <div className="repositoryName">{props.repo.name}</div>
            <div className="visibilityStatus">{props.repo.isPrivate ? "private" : "public"}</div>
            <div className="protectionStatus" onClick={() => 
                HandleProtectionStateChanged(props, setProtection)}>
                {protection ? "protected" : "unprotected"}</div>
        </div>
    );
}

export default Repository;