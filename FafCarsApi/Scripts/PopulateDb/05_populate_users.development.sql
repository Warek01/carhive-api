INSERT INTO "Users" ("Id", "Username", "Password", "Email", "Roles", "DeletedAt", "CreatedAt", "UpdatedAt",
                     "PhoneNumber")
VALUES ('E00E715A-FE5E-4814-B595-6CC3CD316FCA', 'admin', '$2a$13$eG8pcSBpGeyVv0cU0smEKO94K94q5i7OzZG1hbn6EYDaR3T38Mft.',
        'admin@gmai.com', ARRAY ['admin'::USER_ROLE], NULL, '2024-06-15 15:53:58.594', '2024-06-15 15:53:58.594',
        '+37378000111'),
       ('7E4D9D9B-97D8-4E5C-AD49-ABE09837C70C', 'alex', '$2a$13$AI6dIrs7vg0z8VUz6O9y.eOd.oW2lqvGFzln1oym0AyqJjJLFPBRW',
        'alex@gmai.com', ARRAY ['listing_creator'::USER_ROLE], NULL, '2024-06-15 15:53:58.594',
        '2024-06-15 15:53:58.594', '+37378222111'),
       ('29AA0B25-D42A-4877-8B4C-3C359E5BEE77', 'user', '$2a$13$Nd3wMRsS/KCxgowt8omIke37H32zy4RQn7Toa40U.d9P1aEMndGwu',
        'user@gmai.com', ARRAY ['listing_creator'::USER_ROLE], NULL, '2024-06-15 15:53:58.594',
        '2024-06-15 15:53:58.594', '+37378222444');
