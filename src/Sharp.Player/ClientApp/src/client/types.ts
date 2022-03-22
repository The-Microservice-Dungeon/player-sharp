export type FieldId = string;

export type Map = {
    id: string;
    fields: Record<FieldId, Field>;
};

export type Field = {
    movementDifficulty?: number;
    planet?: Planet;
    spaceStation?: SpaceStation;
    connections: FieldId[];
};

export type Planet = {
};

export type SpaceStation = {
};