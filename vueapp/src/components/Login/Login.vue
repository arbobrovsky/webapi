<template>
        <v-container fill-height fluid>
            <v-row align="center"
                   justify="center">
                <v-col>
                    <div class="d-flex justify-center mb-6">
                            <v-card class="elevation-8" min-width="500" >
                                <v-toolbar dark color="blue-darken-1">
                                    <v-toolbar-title>ПС для конвертации валют</v-toolbar-title>
                                </v-toolbar>
                                <form v-on:submit.prevent="login" method="post">
                                    <v-card-text>
                                        <v-text-field prepend-icon="mdi-account"
                                                      :rules="[v => !!v || 'Введите имя пользователя']"
                                                      label="Имя пользователя"
                                                      persistent-placeholder
                                                      id="username"
                                                      name="Username"
                                                      type="text"
                                                      v-model="user.username"
                                                      required></v-text-field>
                                        <v-text-field id="password"
                                                      :rules="[v => !!v && v.length >= 8 || 'Введите пароль']"
                                                      prepend-icon="mdi-lock"
                                                      :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                                                      persistent-placeholder
                                                      label="Пароль"
                                                      name="Password"
                                                      :type="show1 ? 'text' : 'password'"
                                                      @click:append="show1 = !show1"
                                                      v-model="user.password"
                                                      required></v-text-field>
                                    </v-card-text>
                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="success" v-on:click="login" type="submit">Войти</v-btn>
                                        <v-btn color="blue-darken-1" v-on:click="signup" type="submit">Регистрация</v-btn>
                                    </v-card-actions>
                                </form>
                            </v-card>
                      
                    </div>
                </v-col>
            </v-row>
        </v-container>
</template>

<script lang="js">
    import axios from 'axios';
    import Swal from 'sweetalert2';

    export default {
        data() {
            return {
                show1: false,
                user: {
                    username: "",
                    password: ""
                },
            }

        },
        methods: {
            signup() {
                this.$router.push({ name: 'Register' });
            },
            login() {
                if (this.checkValidation()) {
                    axios.get("/api/user/signin/" + this.user.username + "/" + this.user.password)
                        .then(response => {
                            console.log(response.data);
                            if (response.data.userId !== '00000000-0000-0000-0000-000000000000') {
                                console.log(response.data.token);
                                localStorage.setItem('token', JSON.stringify(response.data.token));
                                response.data.token = "";
                                localStorage.setItem('user', JSON.stringify(response.data));
                                this.$router.push({ name: "Dashboard" });
                            } else {
                                Swal.fire("Имя пользователя или пароль введен неверно!");
                            }
                        })
                        .catch(error => {
                            if (error.response) {
                                Swal.fire(error.response.data);
                            }
                        });
                }
            },
            checkValidation() {
                if (!this.user.username) {
                    this.$refs.username.focus();
                    Swal.fire("Введите имя пользователя");
                    return;
                }
                if (!this.user.password) {
                    this.$refs.password.focus();
                    Swal.fire("Введите пароль!");
                    return;
                }
                return true;
            }
        }
    }
</script>
