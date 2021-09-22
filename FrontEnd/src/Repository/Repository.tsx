import { RepositoryModel } from "./RepositoryModel";
import "./repository.css";
import { setRepoProtectionState as setRepoProtectionState } from "../api/GitHubAPIHelpers";
import { useCallback, useState } from "react";

import { ReactComponent as LockOpen } from './assets/lockOpen.svg';
import { ReactComponent as LockClosed } from './assets/lockClosed.svg';


interface repositoryModelProps {
    orgName: string,
    repo: RepositoryModel;
    isProtected: boolean;
}



function Repository(props: repositoryModelProps) {
    const [protection, setProtection] = useState<boolean>(props.repo.isProtected);


    const handleProtectionStateChanged = useCallback(async () => {
        const success = await setRepoProtectionState(props.orgName, props.repo.name, !props.repo.isProtected);
        if (success) {
            setProtection((p) => !p);
        }
    }, [props]);

    return (
        <div className="repoContainer">
            <div className="logoDetailsContainer">
                <div className="logoContainer">
                    <img src="Octocat.png" width="61" height="49" />
                </div>
                <div className="repoDetails">
                    <div>
                        <div className="repositoryHeader">{props.repo.name}</div>
                        <div className="visibilityStatus">{props.repo.isPrivate ? "private" : "public"}</div>
                    </div>

                    <div className="protectionStatus">
                        {protection ? "protected" : "unprotected"}</div>

                </div>
            </div>

            <div className="lockContainer" >
                <div onClick={handleProtectionStateChanged}>
                    {protection ? <LockClosed /> : <LockOpen />}
                </div>
            </div>
        </div>
    );
}

export default Repository;