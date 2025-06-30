
import React, { useState } from 'react';
import Layout from "@/components/layout/Layout";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Badge } from "@/components/ui/badge";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { useAuth } from "@/contexts/AuthContext";
import { useMentorshipMeetings } from "@/hooks/useMentorshipMeetings";
import RescheduleMeetingDialog from "@/components/RescheduleMeetingDialog";
import { Calendar as CalendarIcon, Clock, User, Video, Users, Star } from "lucide-react";
import { format } from "date-fns";
import { ptBR } from "date-fns/locale";
import { cn } from "@/lib/utils";

const Mentorship = () => {
  const { profile } = useAuth();
  const { meetings, createMeeting, updateMeeting, deleteMeeting } = useMentorshipMeetings();
  const [selectedDate, setSelectedDate] = useState<Date>();
  const [selectedTime, setSelectedTime] = useState<string>("");
  const [inviteUser, setInviteUser] = useState<string>("");
  const [rescheduleDialog, setRescheduleDialog] = useState<{
    open: boolean;
    meeting: any;
  }>({ open: false, meeting: null });

  const timeSlots = [
    "09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
    "14:00", "14:30", "15:00", "15:30", "16:00", "16:30",
    "17:00", "17:30", "18:00", "18:30", "19:00", "19:30"
  ];

  const handleScheduleMeeting = async () => {
    if (!selectedDate || !selectedTime || !inviteUser) {
      alert("Por favor, preencha todos os campos para agendar a reunião.");
      return;
    }

    const result = await createMeeting({
      convidado_nome: inviteUser,
      data_reuniao: format(selectedDate, "yyyy-MM-dd"),
      horario: selectedTime
    });

    if (result.success) {
      alert("Reunião agendada com sucesso!");
      setSelectedDate(undefined);
      setSelectedTime("");
      setInviteUser("");
    } else {
      alert("Erro ao agendar reunião. Tente novamente.");
    }
  };

  const handleCancelMeeting = () => {
    setSelectedDate(undefined);
    setSelectedTime("");
    setInviteUser("");
  };

  const handleReschedule = (meeting: any) => {
    setRescheduleDialog({ open: true, meeting });
  };

  if (!profile) {
    return (
      <Layout>
        <div className="p-6">
          <div className="text-center">Carregando...</div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="p-3 md:p-4 space-y-4">
        {/* Header */}
        <div className="flex flex-col md:flex-row md:items-center md:justify-between space-y-3 md:space-y-0">
          <div>
            <h1 className="text-2xl md:text-3xl font-bold">Mentoria</h1>
            <p className="text-muted-foreground text-sm md:text-base">Conecte-se com mentores experientes ou ajude outros desenvolvedores</p>
          </div>
          <div className="flex flex-wrap gap-2">
            <Badge className="bg-gradient-to-r from-blue-600 to-purple-600 text-white text-xs">
              Mentor Nível 2
            </Badge>
            <Badge variant="secondary" className="text-xs">
              {meetings.length} reuniões agendadas
            </Badge>
          </div>
        </div>

        {/* Agendar Nova Reunião */}
        <Card>
          <CardHeader className="pb-3">
            <CardTitle className="flex items-center space-x-2 text-lg">
              <Video className="w-4 h-4" />
              <span>Agendar Nova Reunião</span>
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid md:grid-cols-2 gap-4">
              {/* Formulário de Agendamento */}
              <div className="space-y-3">
                <div className="space-y-2">
                  <Label htmlFor="invite-user" className="text-sm">Convidar Usuário</Label>
                  <Input
                    id="invite-user"
                    placeholder="Digite o nome ou email do usuário"
                    value={inviteUser}
                    onChange={(e) => setInviteUser(e.target.value)}
                  />
                </div>

                <div className="space-y-2">
                  <Label className="text-sm">Data da Reunião</Label>
                  <Popover>
                    <PopoverTrigger asChild>
                      <Button
                        variant="outline"
                        className={cn(
                          "w-full justify-start text-left font-normal",
                          !selectedDate && "text-muted-foreground"
                        )}
                        size="sm"
                      >
                        <CalendarIcon className="mr-2 h-4 w-4" />
                        {selectedDate ? (
                          format(selectedDate, "PPP", { locale: ptBR })
                        ) : (
                          <span>Selecione uma data</span>
                        )}
                      </Button>
                    </PopoverTrigger>
                    <PopoverContent className="w-auto p-0" align="start">
                      <Calendar
                        mode="single"
                        selected={selectedDate}
                        onSelect={setSelectedDate}
                        disabled={(date) => date < new Date()}
                        initialFocus
                        className="pointer-events-auto"
                      />
                    </PopoverContent>
                  </Popover>
                </div>

                <div className="space-y-2">
                  <Label className="text-sm">Horário</Label>
                  <Select value={selectedTime} onValueChange={setSelectedTime}>
                    <SelectTrigger>
                      <SelectValue placeholder="Selecione um horário" />
                    </SelectTrigger>
                    <SelectContent>
                      {timeSlots.map((time) => (
                        <SelectItem key={time} value={time}>
                          <div className="flex items-center space-x-2">
                            <Clock className="w-3 h-3" />
                            <span>{time}</span>
                          </div>
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>

                <div className="flex space-x-2 pt-3">
                  <Button onClick={handleScheduleMeeting} className="flex-1" size="sm">
                    Marcar Reunião
                  </Button>
                  <Button variant="outline" onClick={handleCancelMeeting} className="flex-1" size="sm">
                    Cancelar
                  </Button>
                </div>
              </div>

              {/* Preview da Reunião */}
              <div className="bg-muted p-3 rounded-lg space-y-2">
                <h3 className="font-semibold text-sm">Resumo da Reunião</h3>
                <div className="space-y-1 text-xs">
                  <div className="flex items-center space-x-2">
                    <User className="w-3 h-3 text-muted-foreground" />
                    <span className="text-foreground">Convidado: {inviteUser || "Não selecionado"}</span>
                  </div>
                  <div className="flex items-center space-x-2">
                    <CalendarIcon className="w-3 h-3 text-muted-foreground" />
                    <span className="text-foreground">Data: {selectedDate ? format(selectedDate, "PPP", { locale: ptBR }) : "Não selecionada"}</span>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Clock className="w-3 h-3 text-muted-foreground" />
                    <span className="text-foreground">Horário: {selectedTime || "Não selecionado"}</span>
                  </div>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>

        <div className="grid lg:grid-cols-2 gap-4">
          {/* Próximas Reuniões */}
          <Card>
            <CardHeader className="pb-3">
              <CardTitle className="text-lg">Próximas Reuniões</CardTitle>
            </CardHeader>
            <CardContent className="space-y-3">
              {meetings.length > 0 ? (
                meetings.map((meeting) => (
                  <div key={meeting.id} className="flex items-center justify-between p-3 border rounded-lg">
                    <div className="space-y-1">
                      <h4 className="font-semibold text-sm">{meeting.topico || 'Sessão de Mentoria'}</h4>
                      <p className="text-xs text-muted-foreground">com {meeting.convidado_nome}</p>
                      <div className="flex items-center space-x-2 text-xs text-muted-foreground">
                        <span>{format(new Date(meeting.data_reuniao), "dd/MM/yyyy")}</span>
                        <span>•</span>
                        <span>{meeting.horario}</span>
                        <span>•</span>
                        <span>{meeting.duracao}min</span>
                      </div>
                    </div>
                    <div className="flex flex-col space-y-1">
                      <Button 
                        size="sm" 
                        variant="outline"
                        onClick={() => handleReschedule(meeting)}
                        className="text-xs"
                      >
                        Reagendar
                      </Button>
                      <Button size="sm" className="text-xs">Entrar</Button>
                    </div>
                  </div>
                ))
              ) : (
                <div className="text-center py-6 text-muted-foreground">
                  <Video className="w-10 h-10 mx-auto mb-3 opacity-50" />
                  <p className="text-sm">Nenhuma reunião agendada</p>
                  <p className="text-xs">Agende sua primeira sessão de mentoria!</p>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Mentores Disponíveis */}
          <Card>
            <CardHeader className="pb-3">
              <CardTitle className="text-lg">Mentores Disponíveis</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-center py-6 text-muted-foreground">
                <Users className="w-10 h-10 mx-auto mb-3 opacity-50" />
                <p className="text-sm">Lista de mentores em desenvolvimento</p>
                <p className="text-xs">Em breve você poderá ver mentores disponíveis!</p>
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Estatísticas */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-3">
          <Card>
            <CardContent className="p-3 text-center">
              <div className="text-xl md:text-2xl font-bold text-blue-600">{meetings.filter(m => m.status === 'realizada').length}</div>
              <div className="text-xs text-muted-foreground">Sessões Realizadas</div>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-3 text-center">
              <div className="text-xl md:text-2xl font-bold text-green-600">4.8</div>
              <div className="text-xs text-muted-foreground">Avaliação Média</div>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-3 text-center">
              <div className="text-xl md:text-2xl font-bold text-purple-600">3</div>
              <div className="text-xs text-muted-foreground">Mentores Conectados</div>
            </CardContent>
          </Card>
          <Card>
            <CardContent className="p-3 text-center">
              <div className="text-xl md:text-2xl font-bold text-orange-600">{meetings.length * 60}</div>
              <div className="text-xs text-muted-foreground">Minutos Agendados</div>
            </CardContent>
          </Card>
        </div>

        <RescheduleMeetingDialog
          open={rescheduleDialog.open}
          onOpenChange={(open) => setRescheduleDialog(prev => ({ ...prev, open }))}
          meeting={rescheduleDialog.meeting}
          onReschedule={updateMeeting}
        />
      </div>
    </Layout>
  );
};

export default Mentorship;
