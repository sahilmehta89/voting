import { Injectable } from '@angular/core';

import { AlertingConfig } from 'src/app/core/alerting/alerting-config.model';

import { CommonFunctionsService } from 'src/app/core/common/common-functions.service';
import { VotingAlertingConstants } from '../constants/voting-alerting.constants';



@Injectable()
export class VotingAlertingConfigService {

    constructor(private _commonFunctionsService: CommonFunctionsService) {
    }

    public getAlertingConfigForErrorInAddCandidate(message?: string): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.ErrorTitle,
            message: this._commonFunctionsService.isStringNotNullAndWhiteSpace(message) ? message : VotingAlertingConstants.ErrorMessage_AddCandidate,
        }
        return alertingConfig;
    }

    public getAlertingConfigForErrorInAddVoter(message?: string): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.ErrorTitle,
            message: this._commonFunctionsService.isStringNotNullAndWhiteSpace(message) ? message : VotingAlertingConstants.ErrorMessage_AddVoter,
        }
        return alertingConfig;
    }

    public getAlertingConfigForErrorInCastVote(): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.ErrorTitle,
            message: VotingAlertingConstants.ErrorMessage_CastVote,
        }
        return alertingConfig;
    }

    public getAlertingConfigForErrorInGetCandidates(message?: string): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.ErrorTitle,
            message: VotingAlertingConstants.ErrorMessage_GetCandidates,
        }
        return alertingConfig;
    }

    public getAlertingConfigForErrorInGetVoters(): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.ErrorTitle,
            message: VotingAlertingConstants.ErrorMessage_GetVoters,
        }
        return alertingConfig;
    }

    public getAlertingConfigForErrorInGettingVotersAndCandidates(): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.ErrorTitle,
            message: VotingAlertingConstants.ErrorMessage_GetVotersAndCandidates,
        }
        return alertingConfig;
    }

    public getAlertingConfigForSuccessInAddCandidate(): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.SuccessTitle,
            message: VotingAlertingConstants.SuccessMessage_AddCandidate,
        }
        return alertingConfig;
    }

    public getAlertingConfigForSuccessInAddVoter(): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.SuccessTitle,
            message: VotingAlertingConstants.SuccessMessage_AddVoter,
        }
        return alertingConfig;
    }

    public getAlertingConfigForSuccessInCastVote(): AlertingConfig {
        const alertingConfig: AlertingConfig = {
            title: VotingAlertingConstants.SuccessTitle,
            message: VotingAlertingConstants.SuccessMessage_CastVote,
        }
        return alertingConfig;
    }
}
