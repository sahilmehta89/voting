import { Injectable } from '@angular/core';

import { EnvironmentService } from 'src/app/core/services/config/environment.service';

@Injectable({
    providedIn: 'root'
})
export class VotingConfiguration {

    constructor(private environmentService: EnvironmentService) {
    }

    public getBaseUrl(): string {
        return this.environmentService.getVotingApiBaseUrl();
    }

}
