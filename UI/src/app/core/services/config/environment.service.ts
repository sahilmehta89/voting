import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';

@Injectable()
export class EnvironmentService {

    constructor() { }

    public getVotingApiBaseUrl(): string {
        return environment.votingApiBaseUrl;
    }
}
