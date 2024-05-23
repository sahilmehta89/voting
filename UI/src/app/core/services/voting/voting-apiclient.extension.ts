import { Injectable } from '@angular/core';

import { VotingConfiguration } from 'src/app/core/services/voting/voting-apiclient-config';

@Injectable({
    providedIn: 'root'
})
export class VotingApiClientBase {

    constructor(private votingConfig: VotingConfiguration) {
    }

    protected transformOptions(options: any) {
        return Promise.resolve(options);
    }

    public getBaseUrl(): string {
        return this.votingConfig.getBaseUrl();
    }

}
