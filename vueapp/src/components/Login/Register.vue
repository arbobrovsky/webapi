<template>
    <v-container fill-height fluid>
        <v-card>
            <v-form ref="regForm">
                <v-toolbar dark
                           color="blue-darken-1">
                    <v-toolbar-title>Регистрация пользователя</v-toolbar-title>
                    <v-spacer></v-spacer>
                </v-toolbar>
                <v-row no-gutters>
                    <v-col cols="4">
                        <v-card class="mt-5">
                            <span class="text-h5">Данные для авторизации</span>
                            <v-card-text>
                                <v-text-field prepend-icon="mdi-account"
                                              :rules="[v => v.length >=5 || 'Введите имя пользователя больше 5 символов']"
                                              label="Имя пользователя*"
                                              persistent-placeholder
                                              id="username"
                                              name="Username"
                                              type="text"
                                              v-model="user.username"
                                              required></v-text-field>
                                <v-text-field id="password"
                                              :rules="[v => !!v && v.length >= 8 || 'Введите пароль']"
                                              prepend-icon="mdi-lock"
                                              persistent-placeholder
                                              label="Пароль*"
                                              name="Password"
                                              type="password"
                                              v-model="user.password"
                                              required></v-text-field>
                            </v-card-text>
                        </v-card>
                    </v-col>
                    <v-col cols="8">
                        <v-card class="mt-5">
                            <v-card-title>
                                <span class="text-h5">Личные данные</span>
                            </v-card-title>
                            <v-card-text>
                                <v-row>
                                    <v-col cols="12" padding="6px"
                                           sm="6"
                                           md="6">
                                        <v-text-field clearable v-model="user.firstName"
                                                      label="Имя*"
                                                      :rules="[v => !!v || 'Поле должно быть заполено']"
                                                      required></v-text-field>
                                        <v-text-field clearable v-model="user.lastName"
                                                      :rules="[v => !!v || 'Поле должно быть заполено']"
                                                      required
                                                      label="Фамилия*"></v-text-field>
                                    </v-col>
                                    <v-col cols="12" padding="6px"
                                           sm="6"
                                           md="6">
                                        <v-text-field clearable v-model="user.company"
                                                      :rules="[v => !!v || 'Поле должно быть заполено']"
                                                      required
                                                      label="Организация*"></v-text-field>
                                        <v-text-field clearable v-model="user.location"
                                                      :rules="[v => !!v || 'Поле должно быть заполено']"
                                                      required
                                                      label="Адрес организации*"></v-text-field>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12" padding="6px"
                                           sm="6"
                                           md="6">
                                        <v-text-field clearable v-model="user.email"
                                                      :rules="[ v => /.+@.+/.test(v) || 'Некорректный адрес электронной почты']"
                                                      required
                                                      label="Электронная почта*"></v-text-field>
                                    </v-col>
                                    <v-col cols="12" padding="6px"
                                           sm="6"
                                           md="6">
                                        <v-text-field clearable v-model="user.phone"
                                                      :rules="[v => !!v || 'Поле должно быть заполено']"
                                                      required
                                                      label="Номер телефона*"></v-text-field>

                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12" padding="6px"
                                           sm="12"
                                           md="12">
                                        <v-textarea outlined
                                                    rows="4" v-model="user.bio"
                                                    name="Discription"
                                                    label="О себе"></v-textarea>
                                    </v-col>
                                </v-row>

                            </v-card-text>
                            <v-divider></v-divider>
                            <v-card-actions>
                                <v-btn color="blue-grey-lighten-1" @click="back()">
                                    Назад
                                </v-btn>
                                <v-spacer></v-spacer>
                                <v-btn color="success" @click="signup()">
                                    Регистрация
                                </v-btn>
                            </v-card-actions>
                        </v-card>
                    </v-col>
                </v-row>
            </v-form>
        </v-card>
    </v-container>
</template>


<script>
    import axios from 'axios';
    import Swal from 'sweetalert2';

    export default ({
        data() {
            return {
                user: {
                    userId: 0,
                    username: "",
                    email: "",
                    password: "",
                },
            }
        },
        setup() {

        },
        methods: {
            back() {
                this.$router.push({ name: 'Login' });
            },
            async signup() {
                const { valid } = await this.$refs.regForm.validate();
                //console.log(valid);
                if (valid) {
                    if (this.user !== undefined) {
                        this.user.userId = '00000000-0000-0000-0000-000000000000';
                        this.user.token = "";
                        this.user.message = "";
                        //console.log(this.user);
                        var formData = new FormData();
                        for (var key in this.user) {
                            formData.append(key, this.user[key]);
                        }
                        await axios.post("/api/user/SaveUserFromRegistration", this.user)
                            .then(response => {
                             //   console.log(response.data);
                                if (response.data.message === null) {
                                    //console.log(response.data);
                                    //Swal.fire("Пользователь создан!");
                                    this.back();
                                } else {
                                    Swal.fire(response.data.message);
                                    //Swal.fire("Ошибка : Что-то случилось не так :(");
                                }
                            })
                            .catch(error => {
                                if (error.response) {
                                    Swal.fire(error.response.data);
                                }
                            });
                    }
                }
            },
        }
    })
</script>





