export interface Candidate {
    id: number;
    name: string;
    votes: number;
}

export interface CandidateCreateModel {
    name: string;
}